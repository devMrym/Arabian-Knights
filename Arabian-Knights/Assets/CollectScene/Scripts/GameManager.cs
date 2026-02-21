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
    public float fadeInDuration = 2f;
    public float fadeOutDuration = 2f;
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

        // Ensure fade image starts fully black
        if (sceneFadeImage != null)
        {
            Color c = sceneFadeImage.color;
            c.a = 1f;
            sceneFadeImage.color = c;
            sceneFadeImage.raycastTarget = false;
        }
    }

    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) => ResetLevel();

    void Start()
    {
        ResetLevel();
        ShowTutorial();
        StartCoroutine(StartFadeInNextFrame());
    }

    IEnumerator StartFadeInNextFrame()
    {
        yield return null;
        if (sceneFadeImage != null)
            StartCoroutine(FadeFromBlack());
    }

    void ResetLevel()
    {
        currentTime = gameTime;
        soldiersCollected = 0;
        isPaused = false;
        gameStarted = false;
        isTransitioning = false;

        // Reset SessionData too so retry starts fresh
        if (SessionData.instance != null)
            SessionData.instance.soldiersCollected = 0;

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

                // Save to SessionData before transitioning
                if (SessionData.instance != null)
                    SessionData.instance.soldiersCollected = soldiersCollected;

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

    IEnumerator FadeFromBlack()
    {
        isTransitioning = true;

        sceneFadeImage.color = Color.black;
        sceneFadeImage.raycastTarget = true;
        Canvas.ForceUpdateCanvases();

        yield return null;

        float timer = 0f;
        while (timer < fadeInDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(timer / fadeInDuration);

            Color c = sceneFadeImage.color;
            c.a = 1f - t;
            sceneFadeImage.color = c;

            yield return null;
        }

        Color final = sceneFadeImage.color;
        final.a = 0f;
        sceneFadeImage.color = final;
        sceneFadeImage.raycastTarget = false;
        isTransitioning = false;
    }

    IEnumerator FadeToBlackAndLoad(string sceneName)
    {
        if (isTransitioning) yield break;
        isTransitioning = true;

        Color c = sceneFadeImage.color;
        c.a = 0f;
        sceneFadeImage.color = c;

        float timer = 0f;
        while (timer < fadeOutDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(timer / fadeOutDuration);
            c.a = t;
            sceneFadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        sceneFadeImage.color = c;

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

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

    public void RetryLevel()
    {
        StartCoroutine(FadeToBlackAndLoad(SceneManager.GetActiveScene().name));
    }

    public void BackToMainMenu()
    {
        StartCoroutine(FadeToBlackAndLoad("MainMenu"));
    }
}