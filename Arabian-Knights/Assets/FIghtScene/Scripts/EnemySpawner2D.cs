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

    [Header("UI Overlay")]
    public IntensityOverlayUI intensityOverlay; // drag your Image here in inspector
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
    int maxAttempts = enemyCount * 50; // safety cap

    List<Vector2> usedPositions = new List<Vector2>();

    while (spawned < enemyCount && attempts < maxAttempts)
    {
        attempts++;

        Vector2 randomPos = new Vector2(
            Random.Range(areaCenter.x - areaSize.x / 2, areaCenter.x + areaSize.x / 2),
            Random.Range(areaCenter.y - areaSize.y / 2, areaCenter.y + areaSize.y / 2)
        );

        // Check obstacles
        if (Physics2D.OverlapCircle(randomPos, obstacleCheckRadius, obstacleLayer))
            continue;

        // Optional: prevent enemies spawning on top of each other
        bool tooClose = false;
        foreach (var pos in usedPositions)
        {
            if (Vector2.Distance(pos, randomPos) < obstacleCheckRadius * 2f)
            {
                tooClose = true;
                break;
            }
        }

        if (tooClose)
            continue;

        GameObject enemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);
        usedPositions.Add(randomPos);
        spawned++;

        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        if (health != null)
            health.onDeath.AddListener(OnEnemyKilled);

        EnemyKillBinder binder = enemy.GetComponent<EnemyKillBinder>();
        if (binder != null && intensityOverlay != null)
            binder.intensityOverlay = intensityOverlay;
    }

    if (spawned < enemyCount)
    {
        Debug.LogWarning($"Only spawned {spawned}/{enemyCount}. Area too crowded.");
    }

    if (intensityOverlay != null)
        intensityOverlay.SetTotalEnemies(enemyCount);

    if (killCounterUI != null)
        killCounterUI.SetTotalEnemies(enemyCount);
}


    void OnEnemyKilled()
    {
        if (intensityOverlay != null)
        {
            intensityOverlay.RegisterKill();
        }

        if (killCounterUI != null)
        {
            killCounterUI.RegisterKill();
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
