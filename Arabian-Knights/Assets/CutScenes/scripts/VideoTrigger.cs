using UnityEngine;
using UnityEngine.Video;

public class VideoTrigger : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!videoPlayer.isPlaying)
            {
                videoPlayer.Play();
            }
        }
    }
}
