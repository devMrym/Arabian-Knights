using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class CreditsManager : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeInDuration = 3f;
    public float fadeOutDuration = 2f;
    public float delayBeforeFadeIn = 2f;

    [Header("Music")]
    public AudioSource musicSource;
    public AudioClip creditsMusic;

    void Start()
    {
        StartCoroutine(StartCreditsSequence());
    }

    IEnumerator StartCreditsSequence()
    {
        // Ensure fully black at start
        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;

        // Wait before starting
        yield return new WaitForSeconds(delayBeforeFadeIn);

        // Start music
        if (musicSource != null && creditsMusic != null)
        {
            musicSource.clip = creditsMusic;
            musicSource.loop = true;
            musicSource.Play();
        }

        // Fade from black
        float timer = 0f;
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeInDuration;
            c.a = Mathf.Lerp(1f, 0f, t);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 0f;
        fadeImage.color = c;
    }

    public void BackButton()
    {
        StartCoroutine(FadeOutAndExit());
    }

    IEnumerator FadeOutAndExit()
    {
        float timer = 0f;
        Color c = fadeImage.color;

        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeOutDuration;
            c.a = Mathf.Lerp(0f, 1f, t);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;

        SceneManager.LoadScene("MainMenu");
    }
}