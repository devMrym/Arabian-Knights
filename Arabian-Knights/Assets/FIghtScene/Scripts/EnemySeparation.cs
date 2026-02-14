using UnityEngine;

public class EnemySeparation : MonoBehaviour
{
    public float separationRadius = 0.8f;
    public float separationForce = 1.5f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            separationRadius,
            LayerMask.GetMask("Enemy")
        );

        Vector2 force = Vector2.zero;

        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            Vector2 dir = (Vector2)(transform.position - hit.transform.position);
            float dist = dir.magnitude;

            if (dist > 0.01f)
                force += dir.normalized / dist;
        }

        rb.AddForce(force * separationForce, ForceMode2D.Force);
    }
}
