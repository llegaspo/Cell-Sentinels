using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowButtonAfterDelay : MonoBehaviour
{
    private Button button;
    public float delaySeconds = 6f;

    void Start()
    {
        button = GetComponent<Button>();
        button.interactable = false;
        StartCoroutine(EnableButton());
    }

    IEnumerator EnableButton()
    {
        yield return new WaitForSeconds(delaySeconds);
        button.interactable = true;
    }
}