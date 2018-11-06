using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    Transform player;
    
	void Start ()
    {
        player = transform.root;
        transform.parent = null;
	}
	
	void Update ()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
	}
}
