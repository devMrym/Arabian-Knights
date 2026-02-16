using UnityEngine;
using UnityEngine.UI;

public class IntensityOverlayUI : MonoBehaviour
{
    public Image overlayImage;
    public float pulseSpeed = 2f;        // Speed of opacity pulsing
    public float pulseIntensity = 0.1f;  // How strong the pulse is

    private int totalEnemies = 1;
    private int killedEnemies = 0;
    private float baseAlpha = 0f;

    void Start()
    {
        if (overlayImage != null)
        {
            // Start fully transparent
            Color c = overlayImage.color;
            c.a = 0f;
            overlayImage.color = c;
        }
    }

    void Update()
    {
        if (overlayImage == null) return;

        // Calculate target alpha based on kills
        baseAlpha = (float)killedEnemies / totalEnemies;

        // Apply pulsing on top
        float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseIntensity;

        float finalAlpha = Mathf.Clamp01(baseAlpha + pulse);

        Color c = overlayImage.color;
        c.a = finalAlpha;
        overlayImage.color = c;
    }

    // Called once from spawner to set total enemies
    public void SetTotalEnemies(int total)
    {
        totalEnemies = Mathf.Max(1, total); // avoid division by zero
        killedEnemies = 0;
    }

    // Called every time an enemy dies
    public void RegisterKill()
    {
        killedEnemies = Mathf.Clamp(killedEnemies + 1, 0, totalEnemies);
    }
}
