using UnityEngine;

public class MinimapMove : MonoBehaviour
{
    public float moveSpeed = 5f;

    private SpriteRenderer sprite;

    private Vector2 movement;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // Store movement
        movement = new Vector2(x, y).normalized;


        // Flip character
        if (x > 0.01f)
            sprite.flipX = false; // face right
        else if (x < -0.01f)
            sprite.flipX = true;  // face left
    }

   
}
