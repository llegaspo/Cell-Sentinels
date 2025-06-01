using UnityEngine;
using UnityEngine.Video;

public class VideoEndShowButton : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject button;

    void Start()
    {
        button.SetActive(false); // Hide button initially

        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        button.SetActive(true); // Show button when video ends
    }
}
