using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackRange = 1f;
    public int attackDamage = 10;
    public float attackCooldown = 1f;

    private Transform player;
    private EnemyVision vision;
    private float lastAttackTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        vision = GetComponent<EnemyVision>();
    }

    void Update()
    {
        if (vision.canSeePlayer)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
        }
        else
        {
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            lastAttackTime = Time.time;
        }
    }
}
