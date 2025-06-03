using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TestButtonClick : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        Debug.Log("[TestButtonClick] Listener added on " + gameObject.name);
    }

    void OnClick()
    {
        Debug.Log("[TestButtonClick] Button was clicked!");
    }
}
