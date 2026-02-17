using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GenieShieldUI : MonoBehaviour
{
    public Image icon;       // main shield icon
    public Image radialFill; // spiral image

    private Coroutine pulseRoutine;

    [Header("Opacity Settings")]
    public float inactiveAlpha = 0.4f; // dimmed when shield not ready
    public float readyAlpha = 1f;      // fully visible when ready

    private void SetIconAlpha(float alpha)
    {
        if (icon != null)
        {
            Color c = icon.color;
            c.a = alpha;
            icon.color = c;
        }
    }

    // --- Called when shield is ready to use ---
    public void ShowReady()
    {
        StopPulse();
        radialFill.gameObject.SetActive(false);

        // Fully visible
        SetIconAlpha(readyAlpha);

        StartPulse(); // pulse scale
    }

    // --- Pulse scale only, does not change alpha ---
    private void StartPulse()
    {
        StopPulse();
        pulseRoutine = StartCoroutine(Pulse());
    }

    private void StopPulse()
    {
        if (pulseRoutine != null)
            StopCoroutine(pulseRoutine);
    }

    private IEnumerator Pulse()
    {
        float t = 0f;
        while (true)
        {
            t += Time.deltaTime;
            float scale = 1f + Mathf.Sin(t * 5f) * 0.1f; // small scale pulse
            icon.transform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }
    }

    // --- Shield is active ---
    public IEnumerator ShieldActiveRoutine(float duration)
    {
        radialFill.gameObject.SetActive(true);
        radialFill.fillClockwise = false; // unfilling

        // Dim icon while active
        SetIconAlpha(inactiveAlpha);

        float t = duration;
        while (t > 0)
        {
            t -= Time.deltaTime;
            radialFill.fillAmount = t / duration;
            yield return null;
        }

        radialFill.gameObject.SetActive(false);
    }

    // --- Shield cooldown ---
    public IEnumerator ShieldCooldownRoutine(float cooldown)
    {
        radialFill.gameObject.SetActive(true);
        radialFill.fillClockwise = true; // filling

        // Keep icon dimmed
        SetIconAlpha(inactiveAlpha);

        float t = 0f;
        while (t < cooldown)
        {
            t += Time.deltaTime;
            radialFill.fillAmount = t / cooldown;
            yield return null;
        }

        radialFill.gameObject.SetActive(false);

        // Now ready
        ShowReady();
    }
}
