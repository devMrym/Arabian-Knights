using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private PlayerAnimation anim;

    public Vector2 facingDirection = Vector2.right;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        anim = GetComponent<PlayerAnimation>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        // LEFT / RIGHT facing
        if (movement.x > 0.01f)
            facingDirection = Vector2.right;
        else if (movement.x < -0.01f)
            facingDirection = Vector2.left;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public float GetMovementAmount()
    {
        return movement.magnitude;
    }
}
