using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NeutroSpawnButton : MonoBehaviour
{
    [Header("Which cell prefab to spawn")]
    [Tooltip("Drag the Cell prefab that you want this button to spawn.")]
    public GameObject cellPrefab;

    [Header("DNA cost for this cell")]
    [Tooltip("How many DNA points it costs to spawn this cell.")]
    public int dnaCost = 8;

    private Button uiButton;

    void Awake()
    {
        uiButton = GetComponent<Button>();
        if (uiButton == null)
        {
            Debug.LogError($"[NeutroSpawnButton] No Button component on {gameObject.name}");
            return;
        }

        // Log whether the prefab reference is assigned or not
        if (cellPrefab == null)
            Debug.Log($"[NeutroSpawnButton] On {gameObject.name}, cellPrefab is STILL NULL");
        else
            Debug.Log($"[NeutroSpawnButton] On {gameObject.name}, cellPrefab = {cellPrefab.name}");

        // Add our click listener
        uiButton.onClick.AddListener(OnButtonClicked);
        Debug.Log($"[NeutroSpawnButton] Listener added on {gameObject.name}");
    }

    void OnDestroy()
    {
        if (uiButton != null)
            uiButton.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        Debug.Log("[NeutroSpawnButton] Button was clicked!");
        if (cellPrefab == null)
        {
            Debug.LogWarning($"[NeutroSpawnButton] '{gameObject.name}' has no cellPrefab assigned.");
            return;
        }

        // This will call your GameManager method
        GameManager.Instance.SelectCellToSpawn(cellPrefab, dnaCost);
    }
}
