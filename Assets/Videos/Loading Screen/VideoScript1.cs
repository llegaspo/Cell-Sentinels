using UnityEngine;
using UnityEngine.Video;

public class SeamlessVideoLoop : MonoBehaviour
{
    public double overlapTime = 0.001; // tweak this!

    private VideoPlayer[] players;
    private int currentIndex = 0;

    void Start()
    {
        players = GetComponents<VideoPlayer>();
        if (players.Length < 2)
        {
            Debug.LogError("Need at least 2 VideoPlayers!");
            return;
        }

        // Set up each VideoPlayer
        foreach (var vp in players)
        {
            vp.isLooping = false;
            vp.playOnAwake = false;
        }

        // Register events for BOTH players
        players[0].loopPointReached += OnVideoEnd;
        players[1].loopPointReached += OnVideoEnd;

        // Start with the first VideoPlayer
        players[0].Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        int nextIndex = (currentIndex + 1) % players.Length;

        players[nextIndex].time = 0.1;
        players[nextIndex].Play();

        // Stop the current player after overlap
        Invoke(nameof(StopCurrent), (float)overlapTime);

        // Update current player index
        currentIndex = nextIndex;
    }

    void StopCurrent()
    {
        players[(currentIndex + players.Length - 1) % players.Length].Stop();
    }
}
