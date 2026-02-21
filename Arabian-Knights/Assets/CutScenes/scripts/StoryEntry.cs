using UnityEngine;

[System.Serializable]
public class StoryEntry
{
    public Sprite image;

    [TextArea(2, 5)]
    public string[] texts;

    // Optional colors for each line (must match texts array length)
    public Color[] textColors;
    public Color[] outlineColors;

    // 🔊 NEW: Optional sound per line (same size as texts)
    public AudioClip[] soundEffects;
}