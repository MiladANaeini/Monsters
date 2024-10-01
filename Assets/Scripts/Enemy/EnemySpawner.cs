using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPreFab;

    public float minimumSpawnTime;
    public float maximumSpawnTime;

    private float timeUntilSpawn;
    public PlayingState playingState;

    void Awake()
    {
        SetTimeUntilSpawn();
        playingState = FindObjectOfType<PlayingState>();

        if (playingState == null)
        {
            Debug.LogError("PlayingState not found!");
        }
    }

    void Update()
    {
        if (playingState != null && GamesManager.instance.currentState is PlayingState) { 
        
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn <= 0)
        {
            SpawnEnemy();
            SetTimeUntilSpawn();
        }
        }
    }

    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minimumSpawnTime, maximumSpawnTime);

    }
    private void SpawnEnemy()
    {
        GameObject spawnedEnemy = Instantiate(EnemyPreFab, transform.position, Quaternion.identity);

        Enemy enemyComponent = spawnedEnemy.GetComponent<Enemy>();

        if (enemyComponent != null && playingState != null)
        {
            playingState.AddEnemy(enemyComponent);
        }
        else
        {
            Debug.LogError("Spawned enemy does not have an Enemy component or PlayingState is missing.");
        }
    }
}
