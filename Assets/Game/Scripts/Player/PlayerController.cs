using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public LayerMask movementMask;
    public LayerMask interactable;
    public LayerMask environmentMask;
    public GameObject rayCastPosition;
    public float castRadius;
    public bool wallDetection;

    public Texture2D mainCursor;
    public Texture2D attackCursor;
    public Texture2D lootCursor;

    public ObjectPooling clickToMoveEffect;

    public static bool basicAttack;
    public static bool secondaryAttack;

    Camera cam;
    PlayerMovement movement;
    PlayerLoadout playerLoadout;
    GameObject player;
    Interactable focus;

    List<GameObject> obstruction = new List<GameObject>();
    List<GameObject> walls = new List<GameObject>();

    private void Start()
    {
        cam = Camera.main;
        player = GameObject.Find("Player");
        movement = GetComponent<PlayerMovement>();
        playerLoadout = GetComponent<PlayerLoadout>();

        if(mainCursor)
            Cursor.SetCursor(mainCursor, new Vector2(mainCursor.width / 2, mainCursor.height / 2), CursorMode.Auto);
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) //If you are currently hovering over UI
            return;

        if(!player)
        {
            if (ReferenceManager.player)
                player = ReferenceManager.player;
        }

        if(wallDetection)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(player.transform.position);
            Ray camToPlayer = cam.ScreenPointToRay(screenPos);
            Debug.DrawRay(cam.transform.position, cam.transform.forward * 50, Color.yellow);

            RaycastHit[] wallCollision = Physics.SphereCastAll(cam.transform.position, castRadius, cam.transform.forward, 10f, environmentMask);

            for (int i = 0; i < wallCollision.Length; i++)
            {
                walls.Add(wallCollision[i].transform.gameObject);
            }


            for (int i = 0; i < wallCollision.Length; i++)
            {
                if (wallCollision[i].transform.tag != "Player")
                {
                    if (!obstruction.Contains(wallCollision[i].transform.gameObject))
                    {
                        obstruction.Add(wallCollision[i].transform.gameObject);
                        wallCollision[i].transform.gameObject.GetComponent<VisibilityHandler>().IsVisible(false);
                    }
                }
            }

            for (int j = 0; j < obstruction.Count; j++)
            {
                if (!walls.Contains(obstruction[j].gameObject))
                {
                    obstruction[j].GetComponent<VisibilityHandler>().IsVisible(true);
                    obstruction.Remove(obstruction[j]);
                }
            }

            if (walls.Count > 0)
                walls.Clear();
        }

        if (PlayerMovement.canMove)
        {
            basicAttack = Input.GetKeyDown(KeyCode.Mouse0);
            secondaryAttack = Input.GetKeyDown(KeyCode.Mouse1);
        }

        bool stopWalking = Input.GetKey(KeyCode.LeftShift);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, interactable))
        {
            if (secondaryAttack || basicAttack)
            {
                if (!stopWalking)
                    movement.MoveToPoint(hit.point);

                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    SetFocus(interactable);
                }
            }

            switch (hit.transform.tag)
            {
                case "Enemy":
                    if(attackCursor)
                        Cursor.SetCursor(attackCursor, new Vector2(attackCursor.width / 2, attackCursor.height / 2), CursorMode.Auto);
                    break;
                case "Loot":
                    if(lootCursor)
                        Cursor.SetCursor(lootCursor, new Vector2(lootCursor.width / 2, lootCursor.height / 2), CursorMode.Auto);
                    break;
            }
        }
        else if (Physics.Raycast(ray, out hit, 100, movementMask))
        {
            if(hit.transform.tag.Equals("Ground"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                    if (secondaryAttack || basicAttack)
                    {
                        if (clickToMoveEffect)
                        {
                            GameObject obj = clickToMoveEffect.GetPooledObject();

                            if (obj == null)
                            {
                                return;
                            }

                            obj.transform.position = hit.point;
                            obj.transform.rotation = Quaternion.identity;
                            obj.SetActive(true);
                        }
                    }

                    if (!stopWalking)
                        movement.MoveToPoint(hit.point);

                    RemoveFocus();
                }
            }
            if(mainCursor)
                Cursor.SetCursor(mainCursor, new Vector2(mainCursor.width / 2, mainCursor.height / 2), CursorMode.Auto);
        }

        if (stopWalking)
        {
            movement.agent.SetDestination(transform.position);
        }
    }

    void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if (focus != null)
                focus.OnDefocused();

            focus = newFocus;
            movement.FollowTarget(newFocus);
        }

        playerLoadout.SetFocus(focus);
        newFocus.OnFocused(transform);
    }

    void RemoveFocus()
    {
        if (focus != null)
            focus.OnDefocused();

        focus = null;
        playerLoadout.SetFocus(null);
        movement.StopFollowingTarget();
    }
}
