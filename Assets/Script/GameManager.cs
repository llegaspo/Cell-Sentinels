using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("DNA Settings")]
    public int startingDNA = 30;

    [Header("Battlefield Limits")]
    [Tooltip("Maximum number of cells allowed simultaneously on the field.")]
    public int maxCellsOnField = 10;

    [Header("Cytokinesis Storm Settings")]
    [Tooltip("When currentStormValue >= stormThreshold, the player loses.")]
    public int stormThreshold = 100;

    [Tooltip("Optional parent for spawned cells; leave null if you don’t need this.")]
    public Transform cellsParent;

    public int startingDNAValue => startingDNA;
    public int CurrentDNA { get; private set; }
    public int CurrentStormValue { get; private set; } = 0;

    private GameObject selectedCellPrefab = null;
    private int selectedCellCost = 0;

    private List<GameObject> activeCells = new List<GameObject>();
    private bool levelFinished = false;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        CurrentDNA = startingDNA;
        Debug.Log($"[GameManager] Starting DNA: {CurrentDNA}");
    }

    private void Update()
    {
        if (levelFinished)
            return;

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
    /// Stores which prefab & cost to use (does not clear after spawning).
    /// </summary>
    public void SelectCellToSpawn(GameObject prefab, int cost)
    {
        if (levelFinished)
            return;

        selectedCellPrefab = prefab;
        selectedCellCost = cost;
        Debug.Log($"[GameManager] Selected '{prefab.name}' for spawning. Cost each: {cost} DNA. Remaining DNA: {CurrentDNA}");
    }

    /// <summary>
    /// Attempt to spawn the selected cell prefab at position (deducting DNA and adding storm).
    /// Does not clear selectedCellPrefab, so you can keep spawning until DNA or maxCells limit is reached.
    /// </summary>
    private void TrySpawnSelectedCell(Vector2 position)
    {
        if (selectedCellPrefab == null)
            return; // no selection

        // 1) Check max cell limit
        if (activeCells.Count >= maxCellsOnField)
        {
            Debug.LogWarning($"[GameManager] Cannot spawn more than {maxCellsOnField} cells.");
            return;
        }

        // 2) Check DNA
        if (CurrentDNA < selectedCellCost)
        {
            Debug.LogWarning($"[GameManager] Not enough DNA ({CurrentDNA}) to spawn '{selectedCellPrefab.name}' (cost {selectedCellCost}).");
            return;
        }

        // 3) Deduct DNA
        CurrentDNA -= selectedCellCost;
        Debug.Log($"[GameManager] DNA deducted by {selectedCellCost}. Remaining DNA: {CurrentDNA}");

        // 4) Instantiate the cell
        GameObject newCell = Instantiate(selectedCellPrefab, position, Quaternion.identity);
        if (cellsParent != null)
            newCell.transform.SetParent(cellsParent);
        activeCells.Add(newCell);

        // 5) Read cytokinesisValue from the cell’s SpriteAttributes
        SpriteAttributes cellAttr = newCell.GetComponent<SpriteAttributes>();
        int cytokinesisCost = (cellAttr != null) ? cellAttr.cytokinesisValue : 0;
        CurrentStormValue += cytokinesisCost;
        Debug.Log($"[GameManager] Added {cytokinesisCost} to storm. CurrentStormValue: {CurrentStormValue}/{stormThreshold}");

        OnStormValueChanged();
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

        // Subtract cytokinesis value when cell dies
        SpriteAttributes attr = cellGO.GetComponent<SpriteAttributes>();
        if (attr != null)
        {
            CurrentStormValue -= attr.cytokinesisValue;
            CurrentStormValue = Mathf.Max(0, CurrentStormValue); // prevent going negative
            Debug.Log($"[GameManager] Subtracted {attr.cytokinesisValue} from storm. New value: {CurrentStormValue}/{stormThreshold}");

            OnStormValueChanged(); // Refresh UI and check threshold
        }
    }
}


    /// <summary>
    /// Called whenever CurrentStormValue changes. If we exceed threshold, defeat the player.
    /// </summary>
    private void OnStormValueChanged()
    {
        // Update UI (if you have a CytokinesisBar script—see next section)
        CytokinesisBar.Instance?.RefreshBar();

        if (CurrentStormValue >= stormThreshold && !levelFinished)
        {
            Debug.Log("[GameManager] Cytokinesis Storm threshold reached! YOU LOSE!");
            levelFinished = true;
            // Optionally show a "Game Over" UI here
        }
    }

    /// <summary>
    /// Expose CurrentStormValue normalized (0…1) for UI bars.
    /// </summary>
    public float GetStormNormalized()
    {
        if (stormThreshold <= 0) return 0f;
        return Mathf.Clamp01((float)CurrentStormValue / stormThreshold);
    }

    /// <summary>
    /// Expose CurrentDNA and Starting DNA for UI (like DNABar).
    /// </summary>
    public int GetCurrentDNA() => CurrentDNA;
}
