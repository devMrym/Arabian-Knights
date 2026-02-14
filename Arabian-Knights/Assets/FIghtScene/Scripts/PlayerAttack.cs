using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 1.2f;
    public int attackDamage = 1;
    public LayerMask enemyLayer;
    private PlayerMovement movement;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        UpdateAttackPointDirection();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }


    void Attack()
    {
        Vector2 dir = movement.facingDirection.normalized;
        

        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider2D enemy in enemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
        }
    }


    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void UpdateAttackPointDirection()
    {
        if (movement == null) return;

        Vector2 dir = movement.facingDirection.normalized;
        attackPoint.localPosition = dir;
    }

}
