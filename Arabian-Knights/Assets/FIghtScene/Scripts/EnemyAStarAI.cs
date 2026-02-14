using UnityEngine;
using Pathfinding;

public class EnemyAStarAI : MonoBehaviour
{
    public Transform target;             // Player
    public float detectionRadius = 5f;   // Distance to start chasing
    public float attackRange = 1f;       // Distance to attack
    public int attackDamage = 1;

    public float wanderRadius = 5f;      // Max distance from current position for wandering
    public float wanderCooldown = 2f;    // Time before picking a new wander target
    public float obstacleBuffer = 0.5f;  // Keep enemies away from obstacles

    private AIPath aiPath;
    private Seeker seeker;

    private Vector3 wanderTarget;
    private float wanderTimer;

    void Awake()
    {
        aiPath = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();

        // AIPath settings for top-down 2D
        aiPath.maxSpeed = 3f;
        aiPath.rotationSpeed = 0f;
        aiPath.canMove = true;
        aiPath.canSearch = true;
        aiPath.endReachedDistance = 0.1f;
        aiPath.slowWhenNotFacingTarget = false;
    }

    void Start()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;

        PickNewWanderTarget();
    }

    void Update()
    {
        if (target == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        if (distanceToPlayer <= detectionRadius)
        {
            // Chase player
            aiPath.destination = target.position;
        }
        else
        {
            // Wander when player is far
            wanderTimer -= Time.deltaTime;

            // Pick a new wander target if timer expired or reached old target
            if (wanderTimer <= 0f || Vector2.Distance(transform.position, wanderTarget) < 0.2f)
            {
                PickNewWanderTarget();
            }

            aiPath.destination = wanderTarget;
        }

        // Attack if close
        if (distanceToPlayer <= attackRange)
        {
            Attack();
        }
    }

    void PickNewWanderTarget()
    {
        int maxAttempts = 10;
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomDir = Random.insideUnitCircle * wanderRadius;
            Vector3 candidate = transform.position + new Vector3(randomDir.x, randomDir.y, 0f);

            // Find nearest node on graph
            var nnInfo = AstarPath.active.GetNearest(candidate);
            if (nnInfo.node != null && nnInfo.node.Walkable)
            {
                // Check distance to obstacles using simple raycast
                Vector3 direction = (candidate - transform.position).normalized;
                float distance = Vector3.Distance(candidate, transform.position);
                if (!Physics2D.Raycast(transform.position, direction, distance + obstacleBuffer, LayerMask.GetMask("Obstacles")))
                {
                    wanderTarget = nnInfo.position;
                    wanderTimer = wanderCooldown;
                    return;
                }
            }
        }

        // Fallback: stay in place if no good target found
        wanderTarget = transform.position;
        wanderTimer = wanderCooldown;
    }

    void Attack()
    {
        PlayerHealth player = target.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(attackDamage);
            Debug.Log("Enemy attacked player!");
        }
    }
}
