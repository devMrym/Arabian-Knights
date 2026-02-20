using UnityEngine;

[System.Serializable]
public class StoryEntry
{
    public Sprite image;

    [TextArea(2, 5)]
    public string[] texts;
}
