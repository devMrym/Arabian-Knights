using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pausePanel;

    public static bool IsPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;

        pausePanel.SetActive(IsPaused);
        Time.timeScale = IsPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        IsPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;


        SceneManager.LoadScene("MainMenu"); // hardcoded scene name

    }
}