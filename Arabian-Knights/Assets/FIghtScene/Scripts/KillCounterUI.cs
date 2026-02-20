using UnityEngine;
using TMPro;
using System.Collections;

public class KillCounterUI : MonoBehaviour
{
    public TextMeshProUGUI killText;
    public ScreenFader screenFader; // reference to your screen fader

    private int totalEnemies;
    private int killedEnemies;

    public void SetTotalEnemies(int total)
    {
        totalEnemies = total;
        killedEnemies = 0;
        UpdateText();
    }

    public void RegisterKill()
    {
        killedEnemies++;
        UpdateText();

        if (killedEnemies >= totalEnemies)
        {
            // All enemies killed → start fade
            if (screenFader != null)
                StartCoroutine(FadeAfterDelay(3f)); // optional delay before fading
        }
    }

    void UpdateText()
    {
        killText.text = $"Killed {killedEnemies} / {totalEnemies}";
    }

    private IEnumerator FadeAfterDelay(float delay)
    {
        // Optional: wait before fading, e.g., let player see the final kill
        yield return new WaitForSeconds(delay);

        screenFader.FadeToBlack();
    }
}
