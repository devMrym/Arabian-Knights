using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Timer")]
    public float gameTime = 60f;
    private float currentTime;

    [Header("UI")]
    public int totalSoldiers;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI soldierText;
    public TextMeshProUGUI resultText;
    public GameObject resultPanel;
    public GameObject pausePanel;

    private int soldiersCollected = 0;
    private bool isPaused = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentTime = gameTime;

        resultPanel.SetActive(false);
        pausePanel.SetActive(false);

        totalSoldiers = GameObject.FindGameObjectsWithTag("Soldier").Length;

        UpdateUI();
    }

    void Update()
    {
        if (!isPaused)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                EndGame();
            }

            UpdateTimerUI();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void AddSoldier()
    {
        soldiersCollected++;
        UpdateUI();
        
 
    }

    void UpdateUI()
    {
        soldierText.text = "Soldiers: " + soldiersCollected + " / " + totalSoldiers;
    }

    void UpdateTimerUI()
    {
        timerText.text = "Time: " + Mathf.Ceil(currentTime);
    }

    void EndGame()
    {
        Time.timeScale = 0f;
        resultPanel.SetActive(true);
        resultText.text = "You Collected: " + soldiersCollected + " Soldiers";
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
