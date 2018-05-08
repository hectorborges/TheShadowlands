using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventBridge : MonoBehaviour
{
    PlayerMovement playerMovement;

	void Start ()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
	}
	
    public void Step()
    {
        playerMovement.Step();
    }
}
