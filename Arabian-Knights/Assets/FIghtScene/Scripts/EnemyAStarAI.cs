using UnityEngine;
using Pathfinding;

public class EnemyAStarAI : MonoBehaviour
{
    public Transform target;       // Player transform
    public float nextWaypointDistance = 0.3f;
    public float attackRange = 1f;
    public int attackDamage = 1;

    private AIPath aiPath;
    private Seeker seeker;

    void Awake()
    {
        aiPath = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();

        // Optional: adjust AIPath settings for smooth movement
        aiPath.maxSpeed = 3f;
        aiPath.rotationSpeed = 360f;
        aiPath.canMove = true;
        aiPath.canSearch = true;
        aiPath.endReachedDistance = nextWaypointDistance;
    }

    void Start()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;

        aiPath.destination = target.position;
    }

    void Update()
    {
        if (target == null) return;

        aiPath.destination = target.position;

        // Attack logic
        float distance = Vector2.Distance(transform.position, target.position);
        if (distance <= attackRange)
        {
            Attack();
        }
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
