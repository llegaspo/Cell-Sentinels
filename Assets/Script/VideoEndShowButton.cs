using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class VideoEndShowButton : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject button;
    public float fadeDuration = 8.5f; // time to fully fade in

    private CanvasGroup canvasGroup;

    void Start()
    {
        button.SetActive(true); // Enable for fading
        canvasGroup = button.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0; // Start invisible
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        StartCoroutine(DelayedFadeIn());
    }

    IEnumerator DelayedFadeIn()
    {
        yield return new WaitForSeconds(8.5f); // hardcoded delay

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
