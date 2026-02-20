using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;

public class StoryScene : MonoBehaviour
{
    [Header("UI References")]
    public RawImage storyImage;
    public TextMeshProUGUI storyText;
    public VideoPlayer videoPlayer;

    [Header("Story Data")]
    public StoryEntry[] storyEntries;

    [Header("Video Control")]
    public bool enableVideoInThisScene = false;
    public List<int> videoTriggerEntries = new List<int>();
    // Example: If you put 1 → video plays after entry index 1

    [Header("Timing Settings")]
    public float videoStartDelay = 0f;
    public float fadeDelayAfterVideo = 1.5f;
    public float fadeDuration = 1.5f;

    private int currentEntry = 0;
    private int currentText = 0;
    private bool isLocked = false;

    private Color defaultTextColor;
    private Color defaultOutlineColor;
    private float defaultOutlineWidth;

    void Start()
    {
        storyText.fontMaterial = Instantiate(storyText.fontMaterial);

        defaultTextColor = storyText.color;
        defaultOutlineColor = storyText.fontMaterial.GetColor("_OutlineColor");
        defaultOutlineWidth = storyText.fontMaterial.GetFloat("_OutlineWidth");

        if (storyEntries.Length > 0)
        {
            storyImage.texture = storyEntries[0].image.texture;
            storyImage.color = Color.white;
            currentText = 0;
            ShowLine(0, 0);
        }

        if (videoPlayer != null)
            videoPlayer.loopPointReached += OnVideoFinished;
    }

    void Update()
    {
        if (isLocked)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
            NextLineOrEntry();
    }

    void NextLineOrEntry()
    {
        string[] texts = storyEntries[currentEntry].texts;

        if (currentText < texts.Length - 1)
        {
            currentText++;
            ShowLine(currentEntry, currentText);
        }
        else
        {
            // Decide whether to play video or just fade normally
            if (enableVideoInThisScene && videoTriggerEntries.Contains(currentEntry))
                StartCoroutine(FadeAndPlayVideo());
            else
                StartCoroutine(FadeWithoutVideo());
        }
    }

    void ShowLine(int entryIndex, int textIndex)
    {
        StoryEntry entry = storyEntries[entryIndex];
        storyText.text = entry.texts[textIndex];

        Color textColor = (entry.textColors != null && textIndex < entry.textColors.Length)
            ? entry.textColors[textIndex]
            : defaultTextColor;
        storyText.color = textColor;

        Color outlineColor = (entry.outlineColors != null && textIndex < entry.outlineColors.Length)
            ? entry.outlineColors[textIndex]
            : defaultOutlineColor;

        outlineColor.a = 1f;
        storyText.fontMaterial.SetColor("_OutlineColor", outlineColor);
        storyText.fontMaterial.SetFloat("_OutlineWidth", defaultOutlineWidth);

        storyText.UpdateMeshPadding();
        storyText.SetVerticesDirty();
    }

    IEnumerator FadeAndPlayVideo()
    {
        isLocked = true;

        if (videoStartDelay > 0f)
            yield return new WaitForSeconds(videoStartDelay);

        if (videoPlayer != null)
            videoPlayer.Play();

        yield return new WaitForSeconds(fadeDelayAfterVideo);

        yield return StartCoroutine(FadeImageAndAdvance());

        isLocked = false;
    }

    IEnumerator FadeWithoutVideo()
    {
        isLocked = true;

        yield return StartCoroutine(FadeImageAndAdvance());

        isLocked = false;
    }

    IEnumerator FadeImageAndAdvance()
    {
        float timer = 0f;
        Color startColor = storyImage.color;
        Color endColor = new Color(1, 1, 1, 0);

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;
            storyImage.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        currentEntry++;

        if (currentEntry < storyEntries.Length)
        {
            storyImage.texture = storyEntries[currentEntry].image.texture;
            currentText = 0;
            ShowLine(currentEntry, currentText);

            timer = 0f;
            storyImage.color = new Color(1, 1, 1, 0);

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float t = timer / fadeDuration;
                storyImage.color = Color.Lerp(
                    new Color(1, 1, 1, 0),
                    new Color(1, 1, 1, 1),
                    t);
                yield return null;
            }
        }
        else
        {
            if (videoPlayer != null)
                videoPlayer.Stop();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        isLocked = false;
    }
}
