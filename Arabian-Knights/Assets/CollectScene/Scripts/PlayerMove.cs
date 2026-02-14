using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float Speed = 5f;

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(x, y).normalized;

        transform.position += (Vector3)movement * Speed * Time.deltaTime;
    }
}
