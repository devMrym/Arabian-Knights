using UnityEngine;
using Pathfinding;

public class EnemyAStarAI : MonoBehaviour
{
    public Transform target;               // Player target
    public float detectionRadius = 5f;     // Distance to start chasing
    public float attackRange = 1f;         // Distance to attack
    public int attackDamage = 1;

    public float wanderRadius = 5f;        // Max distance from current position for wandering
    public float wanderCooldown = 2f;      // Time before picking a new wander target
    public float obstacleBuffer = 0.5f;    // Keep enemies away from obstacles

    public float attackCooldown = 1f;      // Time between attacks
    private float lastAttackTime = -Mathf.Infinity;

    [HideInInspector]
    public bool isDead = false;

    private AIPath aiPath;
    private Seeker seeker;
    private Vector3 wanderTarget;
    private float wanderTimer;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        aiPath = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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
        if (target == null || isDead) return; // Stop everything if dead

        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        // Chasing or wandering
        if (distanceToPlayer <= detectionRadius)
            aiPath.destination = target.position;
        else
        {
            wanderTimer -= Time.deltaTime;
            if (wanderTimer <= 0f || Vector2.Distance(transform.position, wanderTarget) < 0.2f)
                PickNewWanderTarget();

            aiPath.destination = wanderTarget;
        }

        // Flip sprite depending on movement direction
        if (aiPath.velocity.x != 0)
            spriteRenderer.flipX = aiPath.velocity.x < 0;

        // Attack if in range
        if (distanceToPlayer <= attackRange && Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }

        // Run animation only if moving, alive, and not attacking
        bool isMoving = aiPath.velocity.magnitude > 0.1f;
        bool isAttackingNow = animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
        animator.SetBool("isRunning", !isDead && isMoving && !isAttackingNow);
    }

    void PickNewWanderTarget()
    {
        int maxAttempts = 10;
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomDir = Random.insideUnitCircle * wanderRadius;
            Vector3 candidate = transform.position + new Vector3(randomDir.x, randomDir.y, 0f);

            var nnInfo = AstarPath.active.GetNearest(candidate);
            if (nnInfo.node != null && nnInfo.node.Walkable)
            {
                Vector3 dir = (candidate - transform.position).normalized;
                float dist = Vector3.Distance(candidate, transform.position);
                if (!Physics2D.Raycast(transform.position, dir, dist + obstacleBuffer, LayerMask.GetMask("Obstacles")))
                {
                    wanderTarget = nnInfo.position;
                    wanderTimer = wanderCooldown;
                    return;
                }
            }
        }

        wanderTarget = transform.position;
        wanderTimer = wanderCooldown;
    }

    void Attack()
    {
        if (isDead) return;

        // Trigger attack animation
        animator.SetTrigger("isAttacking");

        // Deal damage
        PlayerHealth player = target.GetComponent<PlayerHealth>();
        if (player != null && !player.isImmune)
            player.TakeDamage(attackDamage);
    }

    public void StopMovement()
    {
        isDead = true;
        enabled = false;               // stop Update
        if (aiPath != null)
            aiPath.canMove = false;
    }
}
