using UnityEngine;

public class Pushable : MonoBehaviour
{
    public float moveDistance = 1f; // how much the barrel moves per push
    public float moveSpeed = 5f;    // speed of the movement

    private bool isMoving = false;
    private Vector3 targetPos;

    void Update()
    {
        // Smoothly move the barrel if it's moving
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
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
            Vector2 pushDir = (collision.transform.position - transform.position).normalized;
            pushDir = -pushDir; // move away from player

            // Calculate the next position
            targetPos = transform.position + new Vector3(pushDir.x, pushDir.y, 0) * moveDistance;
            isMoving = true;
        }
    }
}
