using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2f;

    void Awake()
    {
        // Ensure starting transparent
        Color c = fadeImage.color;
        c.a = 1f; // Start black
        fadeImage.color = c;
    }

    // Fade from black at scene start
    public IEnumerator FadeFromBlackRoutine()
    {
        yield return StartCoroutine(FadeRoutine(0f));
    }

    // Fade to black when needed
    public IEnumerator FadeToBlackRoutine()
    {
        yield return StartCoroutine(FadeRoutine(1f));
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime; // Works even if Time.timeScale=0
            float a = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);

            Color c = fadeImage.color;
            c.a = a;
            fadeImage.color = c;

            yield return null;
        }

        Color finalColor = fadeImage.color;
        finalColor.a = targetAlpha;
        fadeImage.color = finalColor;
    }
}