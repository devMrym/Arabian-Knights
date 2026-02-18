using UnityEngine;

public class Pushable : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed = 5f;
    public LayerMask obstacleLayer;   // layer for walls, props, barrels, etc.

    private bool isMoving = false;
    private Vector3 targetPos;

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                transform.position = targetPos;
                isMoving = false;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!isMoving && collision.gameObject.CompareTag("Player"))
        {
            Vector2 pushDir =
                (transform.position - collision.transform.position).normalized;

            pushDir = new Vector2(
                Mathf.Round(pushDir.x),
                Mathf.Round(pushDir.y)
            );

            // 🔹 Check if space is free
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                pushDir,
                moveDistance,
                obstacleLayer
            );

            if (hit.collider == null)
            {
                targetPos = transform.position +
                            new Vector3(pushDir.x, pushDir.y, 0) * moveDistance;

                isMoving = true;
            }
        }
    }
}
