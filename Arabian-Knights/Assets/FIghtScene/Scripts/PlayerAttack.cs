// PlayerAttack.cs
using UnityEngine;
public class PlayerAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 1.2f;
    public int attackDamage = 1;
    public LayerMask enemyLayer;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip attackSound;

    private PlayerHealth health;
    private PlayerAnimation anim;
    private PlayerMovement movement;

    void Awake()
    {
        health = GetComponent<PlayerHealth>();
        anim = GetComponent<PlayerAnimation>();
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (health != null && health.isDead) return;
        if (anim != null && anim.IsAttacking) return;
        UpdateAttackPointDirection();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.PlayAttack();
        }
    }

    void UpdateAttackPointDirection()
    {
        if (movement == null || attackPoint == null) return;
        Vector2 dir = movement.facingDirection.normalized;
        attackPoint.localPosition = dir * attackRange;
    }

    public void DealDamage()
    {
        if (audioSource != null && attackSound != null)
            audioSource.PlayOneShot(attackSound);

        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in enemies)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage);
        }
    }
}