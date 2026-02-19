using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private PlayerHealth health;
    private PlayerAnimation anim;

    public Vector2 facingDirection = Vector2.right;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.freezeRotation = true;          // Prevent spinning
        rb.gravityScale = 0f;              // Top-down game

        health = GetComponent<PlayerHealth>();
        anim = GetComponent<PlayerAnimation>();
    }

    void Update()
    {
        // STOP EVERYTHING IF PAUSED
        if (PauseMenu.IsPaused)
        {
            movement = Vector2.zero;
            return;
        }

        // STOP IF DEAD
        if (health != null && health.isDead)
        {
            movement = Vector2.zero;
            return;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        // Update facing direction (Left/Right only)
        if (movement.x > 0.01f)
            facingDirection = Vector2.right;
        else if (movement.x < -0.01f)
            facingDirection = Vector2.left;
    }

    void FixedUpdate()
    {
        if (PauseMenu.IsPaused) return;
        if (health != null && health.isDead) return;

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public float GetMovementAmount()
    {
        return movement.magnitude;
    }
}
