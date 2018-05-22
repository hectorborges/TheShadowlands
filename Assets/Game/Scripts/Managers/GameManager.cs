using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector] public GameObject player;
    PlayerHealth playerHealth;

    private void Awake()
    {
        instance = this;
    }

    public void SetPlayer(GameObject player)
    {
        player = ReferenceManager.instance._player;
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    public void RespawnPlayer()
    {
        print("Respawning... ");
        playerHealth.isDead = false;
        playerHealth.ResetCharacter();
    }
}
