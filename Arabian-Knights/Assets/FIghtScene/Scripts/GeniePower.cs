using System.Collections;
using UnityEngine;

public class GeniePower : MonoBehaviour
{
    public float shieldDuration = 5f;
    public float cooldown = 10f;

    [Header("References")]
    public PlayerHealth health;
    public GameObject shieldVisualPrefab; // world shield prefab under player

    private GameObject shieldVisualInstance;
    private GenieShieldUI shieldUI;
    private bool canUse = true;

    void Start()
    {
        // --- WORLD SHIELD UNDER PLAYER ---
        if (shieldVisualPrefab != null)
        {
            shieldVisualInstance = Instantiate(shieldVisualPrefab, transform);
            shieldVisualInstance.transform.localPosition = new Vector3(0, -0.1f, 0);
            shieldVisualInstance.SetActive(false);
        }

        // --- FIND THE UI IN THE SCENE ---
        shieldUI = FindObjectOfType<GenieShieldUI>();
        if (shieldUI != null)
        {
            shieldUI.ShowReady(); // start pulsing
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canUse)
        {
            StartCoroutine(ActivateShield());
        }
    }

    private IEnumerator ActivateShield()
    {
        canUse = false;
        health.isImmune = true;

        if (shieldVisualInstance != null)
            shieldVisualInstance.SetActive(true);

        if (shieldUI != null)
            StartCoroutine(shieldUI.ShieldActiveRoutine(shieldDuration));

        yield return new WaitForSeconds(shieldDuration);

        health.isImmune = false;

        if (shieldVisualInstance != null)
            shieldVisualInstance.SetActive(false);

        if (shieldUI != null)
            StartCoroutine(shieldUI.ShieldCooldownRoutine(cooldown));

        yield return new WaitForSeconds(cooldown);
        canUse = true;
    }
}
