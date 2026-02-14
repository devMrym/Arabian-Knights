using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 3f;
    public float obstacleAvoidDistance = 0.5f;
    public LayerMask obstacleLayer;
    public float separationDistance = 0.7f;
    public float separationStrength = 2f;

    [Header("Detection & Attack")]
    public float detectionRadius = 6f;
    public float attackRange = 1.2f;
    public float attackCooldown = 1.5f;
    private float nextAttackTime;

    [Header("Wandering")]
    public float wanderChangeTime = 2f;
    private Vector2 wanderDirection;
    private float wanderTimer;

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        PickNewWanderDirection();
    }

    void FixedUpdate()
    {
        Vector2 movement = Vector2.zero;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // ===== Chase player if in detection radius and line-of-sight
        if (distanceToPlayer <= detectionRadius && HasLineOfSight())
        {
            movement = (Vector2)(player.position - transform.position).normalized;
            movement = MoveTowardsWithObstacleAvoidance(movement);
        }
        else
        {
            // ===== Wander smartly
            wanderTimer -= Time.fixedDeltaTime;
            if (wanderTimer <= 0)
                PickNewWanderDirection();

            movement = MoveTowardsWithObstacleAvoidance(wanderDirection);
        }

        // ===== Enemy separation
        movement += Separation() * separationStrength;

        // Normalize and apply
        if (movement != Vector2.zero)
            movement.Normalize();

        rb.linearVelocity = movement * speed;

        // ===== Attack if close
        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    // ===== LINE-OF-SIGHT CHECK
    bool HasLineOfSight()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        float dist = Vector2.Distance(transform.position, player.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist, obstacleLayer);
        return hit.collider == null; // nothing blocking
    }

    // ===== OBSTACLE AVOIDANCE
    Vector2 MoveTowardsWithObstacleAvoidance(Vector2 desiredDir)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(0.4f, 0.4f), 0f, desiredDir, obstacleAvoidDistance, obstacleLayer);

        if (hit.collider == null)
        {
            return desiredDir; // path is clear
        }
        else
        {
            // Slide along obstacle
            Vector2 tangent = Vector2.Perpendicular(desiredDir);

            // Try tangent one side
            if (!Physics2D.BoxCast(transform.position, new Vector2(0.4f, 0.4f), 0f, tangent, obstacleAvoidDistance, obstacleLayer))
                return tangent.normalized;
            // Try opposite side
            if (!Physics2D.BoxCast(transform.position, new Vector2(0.4f, 0.4f), 0f, -tangent, obstacleAvoidDistance, obstacleLayer))
                return (-tangent).normalized;

            // Stuck? stay in place (rare)
            return Vector2.zero;
        }
    }

    // ===== ENEMY SEPARATION
    Vector2 Separation()
    {
        Vector2 separation = Vector2.zero;
        Collider2D[] others = Physics2D.OverlapCircleAll(transform.position, separationDistance, LayerMask.GetMask("Enemy"));
        foreach (Collider2D other in others)
        {
            if (other.gameObject == gameObject) continue;
            Vector2 away = (Vector2)(transform.position - other.transform.position);
            separation += away.normalized / Mathf.Max(away.magnitude, 0.1f);
        }
        return separation;
    }

    void PickNewWanderDirection()
    {
        wanderTimer = wanderChangeTime;
        wanderDirection = Random.insideUnitCircle.normalized;
    }

    void Attack()
    {
        // Example: damage player
        player.GetComponent<PlayerHealth>().TakeDamage(1);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
