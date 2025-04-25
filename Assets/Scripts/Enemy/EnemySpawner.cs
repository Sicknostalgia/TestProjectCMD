using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] baseEnemies;
    public GameObject[] strongerEnemies;
    public float spawnOffset = 2f;
    public float spawnRate = 1.5f;
    private float spawnCountdown;

    public float difficultyInterval = 30f;
    private float difficultyTimer;
    private int difficultyLevel = 0;

    private List<GameObject> currentEnemies = new List<GameObject>();

    public Vector2 arenaCenter = Vector2.zero;
    public Vector2 arenaSize = new Vector2(10, 6);

    void Start()
    {
        spawnCountdown = spawnRate;
        difficultyTimer = 0f;

        currentEnemies.AddRange(baseEnemies); // Start with basic enemies
    }

    void Update()
    {
        spawnCountdown -= Time.deltaTime;
        difficultyTimer += Time.deltaTime;

        if (spawnCountdown <= 0)
        {
            SpawnRandomEnemy();
            spawnCountdown = spawnRate;
        }

        // Add stronger enemies every 3 minutes
        if (difficultyTimer >= difficultyInterval)
        {
            IncreaseDifficulty();
            difficultyTimer = 0f;
        }
    }

    void IncreaseDifficulty()
    {
        if (difficultyLevel < strongerEnemies.Length)
        {
            currentEnemies.Add(strongerEnemies[difficultyLevel]);
            difficultyLevel++;
            Debug.Log("New enemy added to spawn pool!");
        }
    }

    void SpawnRandomEnemy()
    {
        if (currentEnemies.Count == 0) return;

        GameObject randomEnemy = currentEnemies[Random.Range(0, currentEnemies.Count)];
        Vector2 spawnPos = GetRandomSpawnPositionOutsideArena();

        GameObject enemy = ObjectPoolSystem.SpawnObject(randomEnemy, spawnPos, Quaternion.identity);
        if (enemy != null)
        {
            Debug.Log("Spawned enemy at: " + spawnPos);
        }
    }

    Vector2 GetRandomSpawnPositionOutsideArena()
    {
        Vector2 halfSize = arenaSize * 0.5f;
        int edge = Random.Range(0, 4);
        Vector2 spawnPos = Vector2.zero;

        switch (edge)
        {
            case 0: // Top
                spawnPos = new Vector2(
                    Random.Range(arenaCenter.x - halfSize.x, arenaCenter.x + halfSize.x),
                    arenaCenter.y + halfSize.y + spawnOffset);
                break;
            case 1: // Bottom
                spawnPos = new Vector2(
                    Random.Range(arenaCenter.x - halfSize.x, arenaCenter.x + halfSize.x),
                    arenaCenter.y - halfSize.y - spawnOffset);
                break;
            case 2: // Left
                spawnPos = new Vector2(
                    arenaCenter.x - halfSize.x - spawnOffset,
                    Random.Range(arenaCenter.y - halfSize.y, arenaCenter.y + halfSize.y));
                break;
            case 3: // Right
                spawnPos = new Vector2(
                    arenaCenter.x + halfSize.x + spawnOffset,
                    Random.Range(arenaCenter.y - halfSize.y, arenaCenter.y + halfSize.y));
                break;
        }

        return spawnPos;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(arenaCenter, arenaSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(arenaCenter, arenaSize + Vector2.one * spawnOffset * 2);
    }
}
