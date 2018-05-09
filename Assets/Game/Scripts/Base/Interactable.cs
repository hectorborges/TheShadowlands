using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    public Transform interactionTransform;
    public GameObject focusGraphic;

    Transform player;

    bool isFocus = false;
    bool hasInteracted = false;

    public GameObject myMesh;
    Shader originalShader;
    Renderer rend;
    Shader outlineShader;

    void Start()
    {
        rend = myMesh.GetComponent<Renderer>();
        originalShader = rend.material.shader;
        outlineShader = Shader.Find("Custom/OutlineDiffuse");

        rend.material.shader = outlineShader;
        myMesh.GetComponent<Renderer>().materials[0].SetVector("_Color", new Vector4(.5f, .5f, .5f, 0.7019608f));
        rend.material.shader = originalShader;
    }

    private void OnEnable()
    {
        rend = myMesh.GetComponent<Renderer>();
        originalShader = rend.material.shader;
        outlineShader = Shader.Find("Custom/OutlineDiffuse");

        rend.material.shader = outlineShader;
        myMesh.GetComponent<Renderer>().materials[0].SetVector("_Color", new Vector4(.5f, .5f, .5f, 0.7019608f));
        rend.material.shader = originalShader;
    }

    public virtual void Interact()
    {

    }

    private void Update()
    {
        if (isFocus && !hasInteracted)
        {
            float distance = Utility.CheckDistance(player.position, interactionTransform.position);
            if (distance <= radius)
            {
                Interact();
                hasInteracted = true;
            }
        }
    }

    public void OnFocused(Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;

        rend.material.shader = outlineShader;

        if (focusGraphic != null)
            focusGraphic.SetActive(true);
    }

    public void OnDefocused()
    {
        isFocus = false;
        player = null;
        hasInteracted = false;

        rend.material.shader = originalShader;

        if (focusGraphic != null)
            focusGraphic.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
            interactionTransform = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
