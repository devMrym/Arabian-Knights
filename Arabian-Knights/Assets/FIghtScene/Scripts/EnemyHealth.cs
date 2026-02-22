// EnemyHealth.cs
using UnityEngine;
using UnityEngine.Events;
public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public UnityEvent<int, int> onHealthChanged;
    public UnityEvent onDeath;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip dieSound;

    private EnemyAStarAI enemyAI;
    private CapsuleCollider2D capsuleCollider;

    void Awake()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth, maxHealth);
        enemyAI = GetComponent<EnemyAStarAI>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    public void TakeDamage(int damage)
    {
        if (enemyAI != null && enemyAI.isDead) return;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        onDeath?.Invoke();

        if (audioSource != null && dieSound != null)
            audioSource.PlayOneShot(dieSound);

        if (enemyAI != null) enemyAI.StopMovement();

        Animator animator = GetComponent<Animator>();
        if (animator != null) animator.Play("Die", 0, 0f);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) { rb.linearVelocity = Vector2.zero; rb.bodyType = RigidbodyType2D.Static; }

        if (capsuleCollider != null) capsuleCollider.enabled = false;

        Transform minimapIcon = transform.Find("MinimapIcon");
        if (minimapIcon != null)
            minimapIcon.gameObject.SetActive(false);
    }
}