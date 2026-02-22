// GeniePower.cs
using System.Collections;
using UnityEngine;
public class GeniePower : MonoBehaviour
{
    public float shieldDuration = 5f;
    public float cooldown = 10f;

    [Header("References")]
    public PlayerHealth health;
    public GameObject shieldVisualPrefab;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip shieldSound;

    private GameObject shieldVisualInstance;
    private GenieShieldUI shieldUI;
    private bool canUse = true;

    void Start()
    {
        if (shieldVisualPrefab != null)
        {
            shieldVisualInstance = Instantiate(shieldVisualPrefab, transform);
            shieldVisualInstance.transform.localPosition = new Vector3(0, -0.1f, 0);
            shieldVisualInstance.SetActive(false);
        }
        shieldUI = FindObjectOfType<GenieShieldUI>();
        if (shieldUI != null) shieldUI.ShowReady();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canUse)
            StartCoroutine(ActivateShield());
    }

    private IEnumerator ActivateShield()
    {
        canUse = false;
        health.isImmune = true;

        if (audioSource != null && shieldSound != null)
            audioSource.PlayOneShot(shieldSound);

        if (shieldVisualInstance != null) shieldVisualInstance.SetActive(true);
        if (shieldUI != null) StartCoroutine(shieldUI.ShieldActiveRoutine(shieldDuration));

        yield return new WaitForSeconds(shieldDuration);
        health.isImmune = false;
        if (shieldVisualInstance != null) shieldVisualInstance.SetActive(false);
        if (shieldUI != null) StartCoroutine(shieldUI.ShieldCooldownRoutine(cooldown));

        yield return new WaitForSeconds(cooldown);
        canUse = true;
    }
}