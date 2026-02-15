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

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        if (enemyPrefab == null || enemyCount <= 0)
            return;

        int columns = Mathf.CeilToInt(Mathf.Sqrt(enemyCount));
        int rows = Mathf.CeilToInt((float)enemyCount / columns);

        float cellWidth = areaSize.x / columns;
        float cellHeight = areaSize.y / rows;

        List<Vector2> spawnPoints = new List<Vector2>();

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if (spawnPoints.Count >= enemyCount)
                    break;

                Vector2 cellCenter = new Vector2(
                    areaCenter.x - areaSize.x / 2 + cellWidth * (x + 0.5f),
                    areaCenter.y - areaSize.y / 2 + cellHeight * (y + 0.5f)
                );

                Vector2 randomOffset = Random.insideUnitCircle * positionJitter;
                Vector2 spawnPos = cellCenter + randomOffset;

                if (!Physics2D.OverlapCircle(spawnPos, obstacleCheckRadius, obstacleLayer))
                {
                    spawnPoints.Add(spawnPos);
                }
            }
        }

        foreach (Vector2 pos in spawnPoints)
        {
            Instantiate(enemyPrefab, pos, Quaternion.identity);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
#endif
}
