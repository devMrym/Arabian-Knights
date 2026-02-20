using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("CollectScene"); 
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game"); 
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
}
