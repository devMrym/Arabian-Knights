using UnityEngine;

public class SoldierCollect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.AddSoldier();
            Destroy(gameObject);
        }
    }
}
