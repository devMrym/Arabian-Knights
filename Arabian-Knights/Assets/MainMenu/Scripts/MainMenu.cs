using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    public void StartGame()
    {
        StartCoroutine(FadeAndLoad("first-cut-scene"));
    }

    public void OpenCredits()
    {
        StartCoroutine(FadeAndLoad("Credits"));
    }

    public void QuitGame()
    {
        StartCoroutine(FadeAndQuit());
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        yield return StartCoroutine(FadeToBlack());
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeAndQuit()
    {
        yield return StartCoroutine(FadeToBlack());
        Debug.Log("Quit Game");
        Application.Quit();
    }

    IEnumerator FadeToBlack()
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, elapsed / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1;
        fadeImage.color = color;
    }
}