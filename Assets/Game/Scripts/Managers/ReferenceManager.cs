using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    public Camera _mainCamera;
    public GameObject _player;

    public static Camera mainCamera;
    public static GameObject player;

    bool spawnedCharacter;

    void Awake()
    {
        mainCamera = _mainCamera;
    }

    private void Update()
    {
        if(!spawnedCharacter)
        {
            if(GameObject.FindGameObjectWithTag("PlayerSpawnPoint") != null)
            {
                spawnedCharacter = true;
                Transform spawnPosition = GameObject.FindGameObjectWithTag("PlayerSpawnPoint").transform;
                player = Instantiate(_player, spawnPosition.position, spawnPosition.rotation);
            }
        }
    }
}
