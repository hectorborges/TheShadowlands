using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    public Camera _mainCamera;
    public GameObject _player;
    public List<ObjectPooling> _enemyPools;
    public ObjectPooling _floatingCombatTextPool;

    public static Camera mainCamera;
    public static GameObject player;
    public static List<ObjectPooling> enemyPools = new List<ObjectPooling>();
    public static ObjectPooling floatingCombatTextPool;

    bool spawnedCharacter;

    void Awake()
    {
        mainCamera = _mainCamera;
        floatingCombatTextPool = _floatingCombatTextPool;

        for(int i = 0; i < _enemyPools.Count; i++)
        {
            enemyPools.Add(_enemyPools[i]);
        }
    }

    private void Update()
    {
        if(!spawnedCharacter)
        {
            if(GameObject.FindGameObjectWithTag("PlayerSpawnPoint") != null)
            {
                spawnedCharacter = true;
                Transform spawnPosition = GameObject.FindGameObjectWithTag("PlayerSpawnPoint").transform;
                if(_player && spawnPosition)
                    player = Instantiate(_player, spawnPosition.position, spawnPosition.rotation);
            }
        }
    }
}
