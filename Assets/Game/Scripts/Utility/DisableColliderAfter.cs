using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableColliderAfter : MonoBehaviour
{
    public float disableAfter = .1f;
    Collider collision;

    void Start()
    {
        collision = GetComponent<Collider>();
    }

    public void EnableCollider()
    {
        collision.enabled = true;
        StartCoroutine(DisableAfterTime());
    }

    IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(disableAfter);
        collision.enabled = false;
    }
}
