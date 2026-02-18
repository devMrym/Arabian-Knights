using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float Speed = 5f;
    public Animator animator;

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("speed", Mathf.Abs(x));
        animator.SetFloat("speed2", Mathf.Abs(y));


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
