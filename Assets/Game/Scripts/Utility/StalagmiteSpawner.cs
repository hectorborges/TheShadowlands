using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalagmiteSpawner : MonoBehaviour
{
    public GameObject[] stalagmites;
    public Transform[] stalagmiteSpawnPoints;
    public int minimumSpawnCount = 4;
    public int maximumSpawnCount = 10;

    private void Start()
    {
        int spawnCount = Random.Range(minimumSpawnCount, maximumSpawnCount);

        for(int i = 0; i < spawnCount; i++)
        {
            Transform spawnPoint = stalagmiteSpawnPoints[Random.Range(0, stalagmiteSpawnPoints.Length)];
            GameObject randomStalagmite = stalagmites[Random.Range(0, stalagmites.Length)];
            GameObject stalagmite = Instantiate(randomStalagmite, spawnPoint.position, Quaternion.Euler(180,0,0));
            stalagmite.transform.parent = transform;
        }
    }
}
