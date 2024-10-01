using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeLavaSpawner : MonoBehaviour
{
    public GameObject snakeLavaPrefab;

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

    // Update is called once per frame
    void Update()
    {
        if (playingState != null && GamesManager.instance.currentState is PlayingState) { 
        
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn <= 0)
        {
            SpawnSnakeLava();
            SetTimeUntilSpawn();
        }
        }
    }

    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minimumSpawnTime, maximumSpawnTime);

    }
    private void SpawnSnakeLava()
    {
        // Instantiate the enemy prefab
        GameObject spawnedEnemy = Instantiate(snakeLavaPrefab, transform.position, Quaternion.identity);

        // Get the Enemy component from the spawned object
        Enemy enemyComponent = spawnedEnemy.GetComponent<Enemy>();

        if (enemyComponent != null && playingState != null)
        {
            // Add the new enemy to the PlayingState's enemy list
            playingState.AddEnemy(enemyComponent);
        }
        else
        {
            Debug.LogError("Spawned enemy does not have an Enemy component or PlayingState is missing.");
        }
    }
}
