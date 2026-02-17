using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 1.2f;
    public int attackDamage = 1;
    public LayerMask enemyLayer;

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

        // Prevent spamming while attack is in progress
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

    // Called via animation event at hit frame
    public void DealDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider2D enemy in enemies)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage);
        }
    }
}
