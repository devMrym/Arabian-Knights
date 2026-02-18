using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
  //  private PlayerMovement movement;
 

  

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
       

      /*  float speed = movement != null ? movement.GetMovementAmount() : 0f;
        animator.SetFloat("Speed", speed);

        if (movement != null)
            spriteRenderer.flipX = movement.facingDirection.x < 0;*/
    }



}
