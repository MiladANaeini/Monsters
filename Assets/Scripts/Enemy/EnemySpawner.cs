using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPreFab;

    public float minimumSpawnTime;
    public float maximumSpawnTime;

    private float timeUntilSpawn;

    void Awake()
    {
        SetTimeUntilSpawn();
      
    }

   public void UpdateEnemySpawner()
    {
  
        
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn <= 0)
        {
            SpawnEnemy();
            SetTimeUntilSpawn();
        }
        
    }

    private void SetTimeUntilSpawn()
    {
        Debug.Log("minimumSpawnTime" + minimumSpawnTime);
        Debug.Log("maximumSpawnTime" + maximumSpawnTime);
        timeUntilSpawn = Random.Range(minimumSpawnTime, maximumSpawnTime);

    }
    private void SpawnEnemy()
    {
        GameObject spawnedEnemy = ObjectPoolManager.SpawnObject(EnemyPreFab,transform.position,transform.rotation);

        Enemy enemyComponent = spawnedEnemy.GetComponent<Enemy>();

        if (enemyComponent != null)
        {
            PlayingState playingState = FindObjectOfType<PlayingState>();
            if (playingState != null)
            {
                playingState.AddEnemy(enemyComponent); 
            }
        }
        else
        {
            Debug.LogError("Spawned enemy does not have an Enemy component or PlayingState is missing.");
        }
    }
}
