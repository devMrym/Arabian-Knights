using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3f;
    public float detectionRange = 6f;
    public float attackRange = 1.2f;
    public float attackCooldown = 1.5f;

    private Transform player;
    private Rigidbody2D rb;
    private float nextAttackTime;

    private Vector2 wanderDirection;
    private float wanderTimer;
    public float wanderChangeTime = 2f;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            ChasePlayer(distance);
        }
        else
        {
            Wander();
        }
    }

    void ChasePlayer(float distance)
    {
        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * speed;

        if (distance <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void Wander()
    {
        wanderTimer -= Time.fixedDeltaTime;

        if (wanderTimer <= 0)
        {
            PickNewDirection();
        }

        rb.linearVelocity = wanderDirection * speed * 0.6f;
    }


    void Attack()
    {
        player.GetComponent<PlayerHealth>().TakeDamage(1);
    }

    void PickNewDirection()
    {
        wanderTimer = wanderChangeTime;
        wanderDirection = Random.insideUnitCircle.normalized;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            PickNewDirection();
        }
    }
}
    

