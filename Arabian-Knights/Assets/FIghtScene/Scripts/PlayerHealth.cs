using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;
    public bool isImmune = false;
    public bool isDead = false;
    public ScreenFader screenFader;

    public UnityEvent<int, int> onHealthChanged;

    void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isImmune || isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        onHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
            StartCoroutine(Die()); // call coroutine instead of normal method
    }

    private System.Collections.IEnumerator Die()
    {
        if (isDead) yield break;

        isDead = true;
        Debug.Log("Player Died");

        // Stop movement
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // linearVelocity is not valid in Rigidbody2D
            rb.isKinematic = true;
        }

        // Play death animation
        PlayerAnimation anim = GetComponent<PlayerAnimation>();
        if (anim != null)
            anim.PlayDeath();

        // Wait 3 seconds before fading to black
        yield return new WaitForSeconds(3f);

        // Fade to black
        if (screenFader != null)
            screenFader.FadeToBlack();
    }
}
