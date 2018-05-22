using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    public static ReferenceManager instance;
    public Camera _mainCamera;
    public GameObject _player;
    public List<ObjectPooling> _enemyPools;
    public ObjectPooling _floatingCombatTextPool;

    [Space, Header("Ability Pools")]
    public  ObjectPooling _rifleHitEffectPool;
    public ObjectPooling _pistolHitEffectPool;

    public static Camera mainCamera;
    public static GameObject player;
    public static List<ObjectPooling> enemyPools = new List<ObjectPooling>();
    public static ObjectPooling floatingCombatTextPool;
    public static ObjectPooling rifleHitEffectPool;
    public static ObjectPooling pistolHitEffectPool;


    bool spawnedCharacter;

    void Awake()
    {
        instance = this;
        mainCamera = _mainCamera;
        floatingCombatTextPool = _floatingCombatTextPool;
        rifleHitEffectPool = _rifleHitEffectPool;
        pistolHitEffectPool = _pistolHitEffectPool;

        for (int i = 0; i < _enemyPools.Count; i++)
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

                GameManager.instance.SetPlayer(player);
            }
        }
    }
}
