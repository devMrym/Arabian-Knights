using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManagerFight : MonoBehaviour
{
    public static GameManagerFight Instance;

    [Header("UI")]
    public GameObject tutorialPanel;
    public GameObject tryAgainPanel;

    [Header("Fader")]
    public ScreenFader screenFader;

    [Header("Scenes")]
    public string nextSceneName;
    public string mainMenuSceneName;

    private int enemiesAlive = 0;
    private bool levelFinished = false;

    public bool gameStarted { get; private set; } = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    IEnumerator Start()
    {
        Time.timeScale = 0f; // pause game for tutorial

        if (tutorialPanel != null) tutorialPanel.SetActive(false);
        if (tryAgainPanel != null) tryAgainPanel.SetActive(false);

        // Fade from black
        if (screenFader != null)
        {
            yield return new WaitForSecondsRealtime(0.2f);
            yield return StartCoroutine(screenFader.FadeFromBlackRoutine());
        }

        // Show tutorial
        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);
    }

    public void StartGame()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(false);

        gameStarted = true;
        Time.timeScale = 1f;
    }

    public void RegisterEnemy()
    {
        enemiesAlive++;
    }

    public void EnemyKilled()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0 && !levelFinished)
        {
            levelFinished = true;
            StartCoroutine(LevelComplete());
        }
    }

    IEnumerator LevelComplete()
    {
        yield return new WaitForSeconds(1f);

        if (screenFader != null)
            yield return StartCoroutine(screenFader.FadeToBlackRoutine());

        SceneManager.LoadScene(nextSceneName);
    }

    public void PlayerDied()
    {
        StartCoroutine(HandleDeath());
    }

    IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(1f);

        if (screenFader != null)
            yield return StartCoroutine(screenFader.FadeToBlackRoutine());

        if (tryAgainPanel != null)
            tryAgainPanel.SetActive(true);
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}