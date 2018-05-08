using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    Transform player;

	void Start ()
    {
        player = GameObject.Find("Player").transform;
	}
	
	void Update ()
    {
        transform.LookAt(player);
	}
}
