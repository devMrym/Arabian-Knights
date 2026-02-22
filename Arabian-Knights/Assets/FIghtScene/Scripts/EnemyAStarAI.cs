using UnityEngine;
using Pathfinding;
public class EnemyAStarAI : MonoBehaviour
{
    public Transform target;
    public float detectionRadius = 5f;
    public float attackRange = 1f;
    public int attackDamage = 1;
    public float wanderRadius = 5f;
    public float wanderCooldown = 2f;
    public float obstacleBuffer = 0.5f;
    public float attackCooldown = 1f;
    private float lastAttackTime = -Mathf.Infinity;

    [HideInInspector]
    public bool isDead = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip attackSound;

    private AIPath aiPath;
    private Seeker seeker;
    private Vector3 wanderTarget;
    private float wanderTimer;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerHealth playerHealth;

    void Awake()
    {
        aiPath = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = target.GetComponent<PlayerHealth>();
        PickNewWanderTarget();
    }

    void Update()
    {
        if (!GameManagerFight.Instance.gameStarted || isDead) return;
        if (playerHealth != null && playerHealth.isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        if (distanceToPlayer <= detectionRadius)
            aiPath.destination = target.position;
        else
        {
            wanderTimer -= Time.deltaTime;
            if (wanderTimer <= 0f || Vector2.Distance(transform.position, wanderTarget) < 0.2f)
                PickNewWanderTarget();
            aiPath.destination = wanderTarget;
        }

        if (aiPath.velocity.x != 0)
            spriteRenderer.flipX = aiPath.velocity.x < 0;

        if (distanceToPlayer <= attackRange && Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }

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
        if (playerHealth != null && playerHealth.isDead) return;

        if (audioSource != null && attackSound != null)
            audioSource.PlayOneShot(attackSound);

        animator.SetTrigger("isAttacking");
        PlayerHealth player = target.GetComponent<PlayerHealth>();
        if (player != null && !player.isImmune)
            player.TakeDamage(attackDamage);
    }

    public void StopMovement()
    {
        isDead = true;
        enabled = false;
        if (aiPath != null) aiPath.canMove = false;
    }
}