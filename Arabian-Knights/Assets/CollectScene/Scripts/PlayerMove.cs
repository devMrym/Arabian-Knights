using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    public Animator animator;

    private Vector2 movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // Store movement
        movement = new Vector2(x, y).normalized;

        // Send speed to Animator
        animator.SetFloat("speed", movement.magnitude);

        // Flip character
        if (x > 0.01f)
            sprite.flipX = false; // face right
        else if (x < -0.01f)
            sprite.flipX = true;  // face left
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
