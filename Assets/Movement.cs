using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement Variables")]
    public float movementSpeed;
    public float moveToStopRate;
    public float stopToMoveRate;
    public float moveToSprintRate;
    public float sprintSpeed;
    public float sprintToMoveRate;
    public float rotationSpeed;
    public float moveAnimationSmoothTime;
    public float sprintAnimationSmoothTime;
    public float rootMotionForce;

    [Space, Header("Jump Variables")]
    public float jumpForce;
    public float distToGround = 1.1f;
    public float gravity;
    public LayerMask ground;

    [Space, Header("Animations & Effects")]
    //public GameObject landEffect;
    //public AudioSource landSource;
    //public AudioClip[] landClips;
    //public AudioSource jumpSource;
    //public AudioClip[] jumpSounds;
    //public GameObject[] jumpEffects;
    //public GameObject[] leftStepEffects;
    //public GameObject[] rightStepEffects;
    //public AudioSource footstepSource;
    //public AudioClip[] footsteps;

    float speed;
    float horizontal;
    float vertical;

    float horizontal2;

    float xRotationValue;

    Vector3 direction;
    Quaternion rotation;
    Camera cam;

    bool isJumping;
    bool isSprinting;
    int jumpCount;

    int leftStepCount;
    int rightStepCount;

    Rigidbody rb;
    Animator anim;
    Vector3 currentVelocity;
    Transform target;

    public static bool inAir;

    bool combatMovement;
    float newVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        cam = Camera.main;

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void Update()
    {
        //if (Player.AbilityInUse) return;
        RecieveInput();
    }

    private void LateUpdate()
    {
        if(!PlayerLoadout.instance.IsAttacking)
            Move();
        Rotate();
    }

    void RecieveInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        horizontal2 = Input.GetAxis("Mouse X");

        vertical = Input.GetAxis("Vertical");

        isJumping = Input.GetKeyDown(KeyCode.Space);
        isSprinting = Input.GetKey(KeyCode.LeftShift);
    }

    void Move()
    {
        anim.SetFloat("Speed", speed, moveAnimationSmoothTime, Time.deltaTime);

        direction = new Vector3(horizontal, 0, vertical);
        if (isSprinting || direction == Vector3.zero)
        {
            speed = 0;
            rb.velocity = Vector3.zero;
            return;
        }
        direction.Normalize();
        if (direction != Vector3.zero)
        {
            if (speed < movementSpeed)
                speed += stopToMoveRate * Time.deltaTime;
            else if (speed > movementSpeed)
                speed -= sprintToMoveRate * Time.deltaTime;
        }
        else if (speed > 0)
            speed -= moveToStopRate * Time.deltaTime;

            direction *= speed;
        direction = cam.transform.TransformDirection(direction);

        if (direction != Vector3.zero)
            rb.velocity = new Vector3(direction.x, rb.velocity.y, direction.z);
    }

    void CheckGround()
    {
        if (!Grounded())
        {
            Fall();
        }
        else
        {
            if (inAir)
            {
                // landEffect.SetActive(true);
                // landSource.PlayOneShot(landClips[Random.Range(0, landClips.Length)]);

                jumpCount = 0;
                anim.ResetTrigger("Jump");
            }
        }

        inAir = !Grounded();
        anim.SetBool("InAir", inAir);
    }

    public void SetTarget(Interactable newTarget)
    {
        target = newTarget.interactionTransform;
    }

    void Rotate()
    {
        if (PlayerLoadout.instance.IsAttacking)
        {
            combatMovement = true;
            if(target)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
                transform.rotation = lookRotation;
            }
            else
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, ground))
                {
                    Vector3 hitPoint = new Vector3(hit.point.x, 0, hit.point.z);
                    Vector3 direction = (hitPoint - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
                    transform.rotation = lookRotation;
                }
            }
        }
        else
        {
            combatMovement = false;
            Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
            targetDirection = cam.transform.TransformDirection(targetDirection);
            targetDirection.y = 0.0f;
            if (targetDirection != Vector3.zero)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(targetDirection.x, 0, targetDirection.z)), rotationSpeed * Time.deltaTime);
        }
        anim.SetBool("CombatMovement", combatMovement);
    }

    void Jump()
    {
        if (isJumping && Grounded() || isJumping && jumpCount < 2)
        {
            jumpCount++;
            if (jumpCount > 1)
            {
                print("Double Jump");
                anim.SetTrigger("Jump");
            }

            // jumpEffects[jumpCount].SetActive(true);
            // jumpSource.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)]);

            switch (jumpCount)
            {
                case 1:
                    rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                    break;
                case 2:
                    rb.velocity = new Vector3(rb.velocity.x, (jumpForce * 1.5f), rb.velocity.z);
                    break;
            }
        }
    }

    void Fall()
    {
        rb.velocity += Physics.gravity * gravity * Time.fixedDeltaTime;
    }

    public void Step(int foot)
    {
        if (foot == 0)
        {
            // leftStepEffects[leftStepCount].SetActive(true);
            leftStepCount++;

            if (leftStepCount >= 3)
                leftStepCount = 0;
        }
        else
        {
            // rightStepEffects[rightStepCount].SetActive(true);
            rightStepCount++;

            if (rightStepCount >= 3)
                rightStepCount = 0;
        }
        // footstepSource.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)]);
    }

    bool Grounded()
    {
        return Physics.OverlapSphere(transform.position, distToGround, ground).Length > 0;
    }
}
