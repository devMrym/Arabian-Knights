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


        Vector2 movement = new Vector2(x, y).normalized;

        transform.position += (Vector3)movement * Speed * Time.deltaTime;
    }
}
