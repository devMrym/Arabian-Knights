using System.Collections;
using UnityEngine;

public class GeniePower : MonoBehaviour
{
    public float shieldDuration = 5f;
    public float cooldown = 10f;

    private bool canUse = true;
    private PlayerHealth health;

    void Start()
    {
        health = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canUse)
        {
            StartCoroutine(ActivateShield());
        }
    }

    IEnumerator ActivateShield()
    {
        canUse = false;
        health.isImmune = true;
        Debug.Log("Shield ON");

        yield return new WaitForSeconds(shieldDuration);

        health.isImmune = false;
        Debug.Log("Shield OFF");

        yield return new WaitForSeconds(cooldown);
        canUse = true;
        Debug.Log("Shield Ready");
    }
}
