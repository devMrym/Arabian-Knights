using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

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
    public GameObject tutorialPanel;

    [Header("Scene Fade")]
    public Image sceneFadeImage;
    public float sceneFadeDuration = 2f;
    public float resultDisplayTime = 5f;
    public float delayBeforeFade = 3f;
    public string nextSceneName;

    private int soldiersCollected = 0;
    private bool isPaused = false;
    private bool gameStarted = false;
    private bool isTransitioning = false;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) => ResetLevel();

    void Start()
    {
        ResetLevel();
        ShowTutorial();
        StartCoroutine(FadeFromBlack());
    }

    void ResetLevel()
    {
        currentTime = gameTime;
        soldiersCollected = 0;
        isPaused = false;
        gameStarted = false;
        isTransitioning = false;

        Time.timeScale = 0f;

        if (resultPanel != null) resultPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        totalSoldiers = GameObject.FindGameObjectsWithTag("Soldier").Length;

        UpdateUI();
        UpdateTimerUI();
    }

    void ShowTutorial()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(true);
    }

    public void StartGame()
    {
        gameStarted = true;
        Time.timeScale = 1f;
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
    }

    void Update()
    {
        if (!isPaused && gameStarted)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0) EndGame();
            UpdateTimerUI();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && gameStarted)
            TogglePause();
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
        timerText.color = currentTime <= 20f ? Color.red : Color.white;
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
                StartCoroutine(ResultFadeSequence());
            }
        }
    }

    IEnumerator ResultFadeSequence()
    {
        yield return new WaitForSecondsRealtime(resultDisplayTime);
        yield return new WaitForSecondsRealtime(delayBeforeFade);
        yield return StartCoroutine(FadeToBlackAndLoad(nextSceneName));
    }

    // =====================
    // Smooth Fade In / Out
    // =====================

    IEnumerator FadeFromBlack()
    {
        isTransitioning = true;
        float timer = 0f;
        Color color = sceneFadeImage.color;
        color.a = 1f;
        sceneFadeImage.color = color;

        while (timer < sceneFadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(timer / sceneFadeDuration);
            color.a = Mathf.Lerp(1f, 0f, Mathf.SmoothStep(0f, 1f, t));
            sceneFadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        sceneFadeImage.color = color;
        isTransitioning = false;
    }

    IEnumerator FadeToBlackAndLoad(string sceneName)
    {
        if (isTransitioning) yield break;
        isTransitioning = true;

        float timer = 0f;
        Color color = sceneFadeImage.color;
        color.a = 0f;
        sceneFadeImage.color = color;

        while (timer < sceneFadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(timer / sceneFadeDuration);
            color.a = Mathf.Lerp(0f, 1f, Mathf.SmoothStep(0f, 1f, t));
            sceneFadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        sceneFadeImage.color = color;

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    // =====================
    // Pause / Resume
    // =====================

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        pausePanel.SetActive(isPaused);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void QuitGame() => Application.Quit();

    // =====================
    // Lose Panel Buttons with Fade
    // =====================
    public void RetryLevel()
    {
        StartCoroutine(FadeToBlackAndLoad(SceneManager.GetActiveScene().name));
    }

    public void BackToMainMenu()
    {
        StartCoroutine(FadeToBlackAndLoad("MainMenu"));
    }
}