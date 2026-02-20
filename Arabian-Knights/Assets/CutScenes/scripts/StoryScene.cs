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
    public VideoPlayer videoPlayer;

    [Header("Story Data")]
    public StoryEntry[] storyEntries;

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
        // Clone the TMP material to avoid affecting the shared material
        storyText.fontMaterial = Instantiate(storyText.fontMaterial);

        // Store default colors and width from material
        defaultTextColor = storyText.color;
        defaultOutlineColor = storyText.fontMaterial.GetColor("_OutlineColor");
        defaultOutlineWidth = storyText.fontMaterial.GetFloat("_OutlineWidth");

        if (storyEntries.Length > 0)
        {
            // Show first image immediately
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
            StartCoroutine(FadeAndPlayVideo());
        }
    }

    void ShowLine(int entryIndex, int textIndex)
    {
        StoryEntry entry = storyEntries[entryIndex];
        storyText.text = entry.texts[textIndex];

        // Set text color
        Color textColor = (entry.textColors != null && textIndex < entry.textColors.Length)
            ? entry.textColors[textIndex]
            : defaultTextColor;
        storyText.color = textColor;

        // Set outline color (force alpha = 1)
        Color outlineColor = (entry.outlineColors != null && textIndex < entry.outlineColors.Length)
            ? entry.outlineColors[textIndex]
            : defaultOutlineColor;
        outlineColor.a = 1f;
        storyText.fontMaterial.SetColor("_OutlineColor", outlineColor);

        // Ensure outline width is correct
        storyText.fontMaterial.SetFloat("_OutlineWidth", defaultOutlineWidth);

        // Ensure FaceColor alpha > 0 so outline is visible
        Color faceColor = storyText.fontMaterial.GetColor("_FaceColor");
        if (faceColor.a == 0f)
            faceColor.a = 1f;
        storyText.fontMaterial.SetColor("_FaceColor", faceColor);

        // Force TMP to update mesh
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

        float timer = 0f;
        Color startColor = storyImage.color;
        Color endColor = new Color(1, 1, 1, 0);

        // Fade out current image
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
            // Switch to next image (start invisible)
            storyImage.texture = storyEntries[currentEntry].image.texture;
            currentText = 0;
            ShowLine(currentEntry, currentText);

            timer = 0f;
            storyImage.color = new Color(1, 1, 1, 0);

            // Fade in next image
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float t = timer / fadeDuration;
                storyImage.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), t);
                yield return null;
            }
        }
        else
        {
            if (videoPlayer != null)
                videoPlayer.Stop();
        }

        isLocked = false;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        isLocked = false;
    }
}
