using UnityEngine;
using UnityEngine.UI;

public class IntensityOverlayUI : MonoBehaviour
{
    public Image overlayImage;              // The Image to modify
    [Range(0f, 1f)]
    public float pulseStrength = 0.1f;
    [Range(0f, 5f)]
    public float pulseSpeed = 3f;

    private int totalEnemies = 1;
    private int enemiesKilled = 0;
    private float baseAlpha = 0f;

    void Update()
    {
        if (overlayImage == null) return;

        // Continuous pulse
        float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseStrength;
        float finalAlpha = Mathf.Clamp01(baseAlpha + pulse);

        Color c = overlayImage.color;
        c.a = finalAlpha;
        overlayImage.color = c;
    }

    // Call this at the start to tell overlay how many enemies there are
    public void SetTotalEnemies(int count)
    {
        totalEnemies = Mathf.Max(1, count);
        UpdateBaseAlpha();
    }

    // Call this whenever an enemy dies
    public void RegisterKill()
    {
        enemiesKilled++;
        UpdateBaseAlpha();
    }

    private void UpdateBaseAlpha()
    {
        baseAlpha = Mathf.Clamp01((float)enemiesKilled / totalEnemies);
    }
}
