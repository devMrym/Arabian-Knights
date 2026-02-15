using UnityEngine;

public class EnemyHealthBarBinder : MonoBehaviour
{
    public HealthBar healthBarPrefab;

    void Start()
    {
        EnemyHealth health = GetComponent<EnemyHealth>();
        HealthBar bar = Instantiate(healthBarPrefab);

        bar.Follow(transform);
        bar.UpdateBar(health.currentHealth, health.maxHealth);

        health.onHealthChanged.AddListener(bar.UpdateBar);
    }
}
