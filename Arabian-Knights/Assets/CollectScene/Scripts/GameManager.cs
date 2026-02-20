using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
    public GameObject losePanel;

    [Header("Tutorial")]
    public GameObject tutorialPanel; // assign in inspector
   // public GameObject gameplayUI;    // the UI for timer, soldier count, etc.

    private int soldiersCollected = 0;
    private bool isPaused = false;
    private bool gameStarted = false; // controls when gameplay begins

    void Awake()
    {
        // Singleton protection
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetLevel(); // reset timer, soldiers, UI
    }

    void Start()
    {
        ResetLevel();
        ShowTutorial();
    }

    void ResetLevel()
    {
        // Reset timer and collected soldiers
        currentTime = gameTime;
        soldiersCollected = 0;
        isPaused = false;
        gameStarted = false;
        Time.timeScale = 0f; // pause until player starts game

        // Reset UI panels
        if (resultPanel != null) resultPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        // Count soldiers in scene
        totalSoldiers = GameObject.FindGameObjectsWithTag("Soldier").Length;

        UpdateUI();
        UpdateTimerUI();
    }

    void ShowTutorial()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(true);
      //  if (gameplayUI != null) gameplayUI.SetActive(false);
    }

    public void StartGame()
    {
        gameStarted = true;
        Time.timeScale = 1f; // resume game

        if (tutorialPanel != null) tutorialPanel.SetActive(false);
      //  if (gameplayUI != null) gameplayUI.SetActive(true);
    }

    void Update()
    {
        if (!isPaused && gameStarted)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                EndGame();
            }

            UpdateTimerUI();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && gameStarted)
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
        if (currentTime <= 20f)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = Color.white; // normal color
        }
    }

    void EndGame()
    {
        Time.timeScale = 0f;

        if (soldiersCollected <= 0)
        {
            if (losePanel != null)
                losePanel.SetActive(true);
        }
        else
        {
            if (resultPanel != null)
            {
                resultPanel.SetActive(true);
                resultText.text = "You Collected: " + soldiersCollected + " Soldiers";
            }
        }
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

    // ===========================
    // Lose Panel Buttons
    // ===========================

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
