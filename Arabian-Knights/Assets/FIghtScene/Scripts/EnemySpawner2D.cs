using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner2D : MonoBehaviour
{
    [Header("Enemy")]
    public GameObject enemyPrefab;
    public int enemyCount = 10;

    [Header("Spawn Area (Rectangle)")]
    public Vector2 areaCenter;
    public Vector2 areaSize = new Vector2(20, 10);

    [Header("Placement Rules")]
    public LayerMask obstacleLayer;
    public float obstacleCheckRadius = 0.4f;
    public float positionJitter = 0.4f;

    [Header("Kill Counter UI")]
    public KillCounterUI killCounterUI;

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        int spawned = 0;
        int attempts = 0;
        int maxAttempts = enemyCount * 50;

        List<Vector2> usedPositions = new List<Vector2>();

        while (spawned < enemyCount && attempts < maxAttempts)
        {
            attempts++;

            Vector2 randomPos = new Vector2(
                Random.Range(areaCenter.x - areaSize.x / 2, areaCenter.x + areaSize.x / 2),
                Random.Range(areaCenter.y - areaSize.y / 2, areaCenter.y + areaSize.y / 2)
            );

            if (Physics2D.OverlapCircle(randomPos, obstacleCheckRadius, obstacleLayer))
                continue;

            bool tooClose = false;
            foreach (var pos in usedPositions)
            {
                if (Vector2.Distance(pos, randomPos) < obstacleCheckRadius * 2f)
                {
                    tooClose = true;
                    break;
                }
            }
            if (tooClose) continue;

            GameObject enemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);
            usedPositions.Add(randomPos);
            spawned++;

            GameManagerFight.Instance.RegisterEnemy();

            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            if (health != null)
                health.onDeath.AddListener(OnEnemyKilled);
        }

        if (killCounterUI != null)
            killCounterUI.SetTotalEnemies(enemyCount);
    }

    void OnEnemyKilled()
    {
        GameManagerFight.Instance.EnemyKilled();
        if (killCounterUI != null)
            killCounterUI.RegisterKill();
    }
}