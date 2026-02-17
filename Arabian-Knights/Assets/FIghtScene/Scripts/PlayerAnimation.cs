using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerMovement movement;
    private PlayerHealth health;

    public bool IsAttacking { get; private set; }

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<PlayerMovement>();
        health = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (health != null && health.isDead)
        {
            animator.SetBool("IsDead", true);
            animator.SetFloat("Speed", 0);
            return;
        }

        float speed = movement != null ? movement.GetMovementAmount() : 0f;
        animator.SetFloat("Speed", speed);

        if (movement != null)
            spriteRenderer.flipX = movement.facingDirection.x < 0;
    }

    // Called from PlayerAttack when Space is pressed
    public void PlayAttack()
    {
        if (health != null && health.isDead) return;

        animator.SetTrigger("Attack");
        IsAttacking = true; // blocks new attack until animation event resets it
    }

    // Called via animation event at last frame of attack
    public void OnAttackEnd()
    {
        IsAttacking = false;
        Debug.Log("Attack ended, IsAttacking = " + IsAttacking);
    }



    public void PlayDeath()
    {
        if (animator == null) return;
        animator.SetBool("IsDead", true);
    }



}
