using UnityEngine;
using TMPro;

public class KillCounterUI : MonoBehaviour
{
    public TextMeshProUGUI killText;

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
    }

    void UpdateText()
    {
        killText.text = $"Killed {killedEnemies} / {totalEnemies}";
    }
}
