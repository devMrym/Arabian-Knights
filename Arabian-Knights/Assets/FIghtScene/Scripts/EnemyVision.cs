using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public float viewRadius = 5f;
    public LayerMask playerLayer;
    public bool canSeePlayer;

    void Update()
    {
        Collider2D player = Physics2D.OverlapCircle(
            transform.position,
            viewRadius,
            playerLayer
        );

        canSeePlayer = player != null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
}
