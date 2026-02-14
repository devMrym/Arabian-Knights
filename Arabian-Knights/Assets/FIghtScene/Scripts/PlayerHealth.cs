using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;
    public bool isImmune = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isImmune) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Debug.Log("Player Died");
        }
    }
}
