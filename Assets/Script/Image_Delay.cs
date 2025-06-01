using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowImageAfterDelay : MonoBehaviour
{
    private Image image;
    public float delaySeconds = 2f;

    void Start()
    {
        image = GetComponent<Image>();
        image.enabled = false;
        StartCoroutine(ShowImage());
    }

    IEnumerator ShowImage()
    {
        yield return new WaitForSeconds(delaySeconds);
        image.enabled = true;
    }
}
