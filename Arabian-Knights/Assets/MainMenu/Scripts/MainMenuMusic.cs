using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip menuMusic;

    void Start()
    {
        if (musicSource != null && menuMusic != null)
        {
            musicSource.clip = menuMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
}