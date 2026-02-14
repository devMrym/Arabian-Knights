using UnityEngine;

public class Lever : MonoBehaviour
{
    public Door door;
    bool used = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !used)
        {
            used = true;
            door.OpenDoor();
        }
    }
}
