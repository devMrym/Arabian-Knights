using UnityEngine;
using UnityEngine.Events;
using System.Collections;

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
            StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        if (isDead) yield break;

        isDead = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
        }

        PlayerAnimation anim = GetComponent<PlayerAnimation>();
        if (anim != null)
            anim.PlayDeath();

        yield return new WaitForSecondsRealtime(3f);

        if (screenFader != null)
            yield return StartCoroutine(screenFader.FadeToBlackRoutine());

        GameManagerFight.Instance.PlayerDied();
    }
}