using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableColliderAfter : MonoBehaviour
{
    public float disableAfter = .1f;
    Collider collider;

    void Start()
    {
        collider = GetComponent<Collider>();
    }

    public void EnableCollider()
    {
        collider.enabled = true;
        StartCoroutine(DisableAfterTime());
    }

    IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(disableAfter);
        collider.enabled = false;
    }
}
