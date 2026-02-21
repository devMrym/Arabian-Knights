using UnityEngine;
using TMPro;
using System.Collections;

public class KillCounterUI : MonoBehaviour
{
    public TextMeshProUGUI killText;  // Must be assigned in inspector
    public ScreenFader screenFader;

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
            if (screenFader != null)
                StartCoroutine(FadeAfterDelay(1f)); // optional short delay
        }
    }

    private void UpdateText()
    {
        if (killText != null)
            killText.text = $"Kills: {killedEnemies} / {totalEnemies}";
    }

    private IEnumerator FadeAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // works even if game is paused
        if (screenFader != null)
            yield return StartCoroutine(screenFader.FadeToBlackRoutine());
    }
}