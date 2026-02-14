using UnityEngine;

public class HideZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.layer = LayerMask.NameToLayer("Hidden");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }
}
