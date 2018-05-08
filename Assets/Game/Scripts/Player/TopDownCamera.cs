using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float followSpeed;

    Vector3 velocity;

	void LateUpdate ()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.transform.position + offset, ref velocity, followSpeed);
	}
}
