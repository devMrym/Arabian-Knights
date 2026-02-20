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
        c.a = 0f;
        fadeImage.color = c;
    }

    public void FadeToBlack()
    {
        StartCoroutine(FadeRoutine(1f));
    }

    IEnumerator FadeRoutine(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
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
