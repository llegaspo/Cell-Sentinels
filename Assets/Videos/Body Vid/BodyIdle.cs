using UnityEngine;
using UnityEngine.Video;

public class SeamlessVideoPlayer : MonoBehaviour
{
    public VideoClip firstClip;
    public VideoClip loopClip;

    private VideoPlayer videoPlayer;
    private bool hasSwitched = false;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.None;

        videoPlayer.clip = firstClip;
        videoPlayer.isLooping = false;
        videoPlayer.loopPointReached += OnFirstClipEnded;

        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += vp => videoPlayer.Play();
    }

    void OnFirstClipEnded(VideoPlayer vp)
    {
        if (hasSwitched) return;

        hasSwitched = true;

        videoPlayer.clip = loopClip;
        videoPlayer.isLooping = true;

        videoPlayer.Prepare(); // async preload
        videoPlayer.prepareCompleted += vp2 => videoPlayer.Play();
    }
}
