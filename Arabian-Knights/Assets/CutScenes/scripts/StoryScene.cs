using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using System.Collections;

public class StoryScene : MonoBehaviour
{
    [Header("UI References")]
    public RawImage storyImage;
    public TextMeshProUGUI storyText;
    public Image fadeImage;
    public VideoPlayer videoPlayer;

    [Header("Story Data")]
    public StoryEntry[] storyEntries;

    [Header("Timing Settings")]
    public float videoStartDelay = 0f;
    public float fadeDelayAfterVideo = 1.5f;
    public float fadeDuration = 1.5f;

    private int currentEntry = 0;
    private int currentText = 0;
    private bool isLocked = false;   // locks ALL input during transition + video

    void Start()
    {
        if (storyEntries.Length > 0)
            ShowEntry(currentEntry);

        if (fadeImage != null)
            fadeImage.color = new Color(0, 0, 0, 0);

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    void Update()
    {
        // Block ALL input if locked
        if (isLocked)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLineOrEntry();
        }
    }

    void ShowEntry(int index)
    {
        currentText = 0;

        storyImage.texture = storyEntries[index].image.texture;
        storyText.text = storyEntries[index].texts[currentText];

        storyImage.color = Color.white;
    }

    void NextLineOrEntry()
    {
        string[] texts = storyEntries[currentEntry].texts;

        if (currentText < texts.Length - 1)
        {
            currentText++;
            storyText.text = texts[currentText];
        }
        else
        {
            StartCoroutine(FadeAndPlayVideo());
        }
    }

    IEnumerator FadeAndPlayVideo()
    {
        isLocked = true;   // 🔒 LOCK INPUT

        // Optional delay before video starts
        if (videoStartDelay > 0f)
            yield return new WaitForSeconds(videoStartDelay);

        // ▶ Start video FIRST
        if (videoPlayer != null)
            videoPlayer.Play();

        // Wait before fade begins (video continues playing)
        yield return new WaitForSeconds(fadeDelayAfterVideo);

        float timer = 0f;

        Color imageStart = storyImage.color;
        Color imageEnd = new Color(1, 1, 1, 0);

        Color fadeStart = fadeImage.color;
        Color fadeEnd = new Color(0, 0, 0, 1);

        // Fade OUT current image
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            storyImage.color = Color.Lerp(imageStart, imageEnd, t);
            fadeImage.color = Color.Lerp(fadeStart, fadeEnd, t);

            yield return null;
        }

        currentEntry++;

        if (currentEntry < storyEntries.Length)
        {
            // Setup next entry
            storyImage.texture = storyEntries[currentEntry].image.texture;
            storyText.text = storyEntries[currentEntry].texts[0];
            currentText = 0;

            timer = 0f;

            storyImage.color = new Color(1, 1, 1, 0);
            fadeImage.color = new Color(0, 0, 0, 1);

            // Fade IN next image (video still playing)
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float t = timer / fadeDuration;

                storyImage.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, t);
                fadeImage.color = Color.Lerp(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), t);

                yield return null;
            }
        }
        else
        {
            // End of story
            if (videoPlayer != null)
                videoPlayer.Stop();
        }

        // DO NOT unlock here — wait for video to finish
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        isLocked = false;  // 🔓 UNLOCK when video completely ends
    }
}
