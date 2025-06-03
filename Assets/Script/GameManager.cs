using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Starting DNA (integer)")]
    public int startingDNA = 30;

    [Tooltip("Optional parent for spawned cells; leave null if you don’t need this.")]
    public Transform cellsParent;

    // —— Internal State —— //
    private GameObject selectedCellPrefab = null;
    private int selectedCellCost = 0;

    private int currentDNA;
    private List<GameObject> activeCells = new List<GameObject>();

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Initialize DNA
        currentDNA = startingDNA;
        Debug.Log($"[GameManager] Starting DNA: {currentDNA}");
    }

    private void Update()
    {
        // If no cell type has been selected yet, do nothing
        if (selectedCellPrefab == null)
            return;

        // Every left‐mouse click attempts to spawn (as long as not over UI)
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 spawnPos = new Vector2(worldPos.x, worldPos.y);

            TrySpawnSelectedCell(spawnPos);
        }
    }

    /// <summary>
    /// Called by CellButton when its UI button is clicked.
    /// Stores which prefab & cost to use (but does NOT clear after spawning).
    /// </summary>
    public void SelectCellToSpawn(GameObject prefab, int cost)
    {
        selectedCellPrefab = prefab;
        selectedCellCost = cost;
        Debug.Log($"[GameManager] Selected '{prefab.name}' for spawning. Cost each: {cost} DNA. Remaining DNA: {currentDNA}");
    }

    /// <summary>
    /// Attempt to spawn the selected cell prefab at position (deducting DNA). 
    /// Does not clear selectedCellPrefab, so you can keep spawning until DNA runs out.
    /// </summary>
    private void TrySpawnSelectedCell(Vector2 position)
    {
        if (selectedCellPrefab == null)
            return; // no selection

        if (currentDNA < selectedCellCost)
        {
            Debug.LogWarning($"[GameManager] Not enough DNA ({currentDNA}) to spawn '{selectedCellPrefab.name}' (cost {selectedCellCost}).");
            return;
        }

        // Deduct DNA, update
        currentDNA -= selectedCellCost;
        Debug.Log($"[GameManager] Spawned '{selectedCellPrefab.name}' at {position}. DNA left: {currentDNA}");

        // Instantiate the cell
        GameObject newCell = Instantiate(selectedCellPrefab, position, Quaternion.identity);

        if (cellsParent != null)
            newCell.transform.SetParent(cellsParent);

        activeCells.Add(newCell);

        // *** Do NOT clear selectedCellPrefab or selectedCellCost here ***
        // That way, clicks keep spawning more until DNA is insufficient.
    }

    /// <summary>
    /// Called by SpriteAttributes when a cell GameObject dies.
    /// </summary>
    public void OnCellDeath(GameObject cellGO)
    {
        if (activeCells.Contains(cellGO))
        {
            activeCells.Remove(cellGO);
            Debug.Log($"[GameManager] A cell died. {activeCells.Count} cells still alive.");
        }
    }
    public int GetCurrentDNA()
{
    return currentDNA;
}public void AddDNA(int amount)
{
    currentDNA = Mathf.Clamp(currentDNA + amount, 0, startingDNA);
}


}
