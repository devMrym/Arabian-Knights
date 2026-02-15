using UnityEngine;

public class PlayerHealthBarBinder : MonoBehaviour
{
    public HealthBar healthBarPrefab;

    void Start()
    {
        PlayerHealth health = GetComponent<PlayerHealth>();
        HealthBar bar = Instantiate(healthBarPrefab);

        bar.Follow(transform);
        bar.UpdateBar(health.currentHealth, health.maxHealth);

        health.onHealthChanged.AddListener(bar.UpdateBar);
    }
}
