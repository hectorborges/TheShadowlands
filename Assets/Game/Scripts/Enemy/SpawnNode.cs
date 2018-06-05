using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNode : MonoBehaviour
{
    public List<ObjectPooling> enemies;
    public int minimumSpawn;
    public int maximumSpawn;

    List<Vector3> spawnPoints = new List<Vector3>();

    bool spawned;

    private void Start()
    {
        for (int i = 0; i < ReferenceManager.enemyPools.Count; i++)
            enemies.Add(ReferenceManager.enemyPools[i]);

        AddSpawnPoints();
    }

    void SpawnEnemies()
    {
        int spawnCount = Random.Range(minimumSpawn, maximumSpawn);


        for(int i = 0; i < spawnCount; i++)
        {
            ObjectPooling randomPool = enemies[Random.Range(0, enemies.Count)];
            GameObject obj = randomPool.GetPooledObject();

            if (obj == null)
            {
                return;
            }

            Vector3 spawnLocation = spawnPoints[Random.Range(0, spawnPoints.Count)];

            obj.transform.position = transform.position + spawnLocation;
            obj.transform.rotation = Quaternion.identity;
            obj.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player") && !spawned)
        {
            spawned = true;
            SpawnEnemies();
        }
    }

    void AddSpawnPoints()
    {
        spawnPoints.Add(new Vector3(0, 0, 0));  //Middle

        spawnPoints.Add(new Vector3(0, 0, 1));  //Forward
        spawnPoints.Add(new Vector3(-1, 0, 1));  //Forward Left
        spawnPoints.Add(new Vector3(1, 0, 1));  //Forard Right

        spawnPoints.Add(new Vector3(-1, 0, 0));  //Left
        spawnPoints.Add(new Vector3(1, 0, 0));  //Right

        spawnPoints.Add(new Vector3(0, 0, -1));  //Backward
        spawnPoints.Add(new Vector3(-1, 0, -1));  //Backward Left
        spawnPoints.Add(new Vector3(1, 0, -1));  //Backawrd Right
    }
}
