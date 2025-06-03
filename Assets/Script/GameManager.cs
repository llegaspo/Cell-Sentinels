using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("DNA Settings")]
    public int startingDNA = 30;

    [Header("Battlefield Limits")]
    public int maxCellsOnField = 10;

    [Header("Cytokinesis Storm Settings")]
    public int stormThreshold = 100;

    [Tooltip("Optional parent for spawned cells; leave null if you don’t need this.")]
    public Transform cellsParent;

    [Header("Result UI")]
    public GameObject victoryPanel;
    public GameObject defeatPanel;

    public int startingDNAValue => startingDNA;
    public int CurrentDNA { get; private set; }
    public int CurrentStormValue { get; private set; } = 0;

    private GameObject selectedCellPrefab = null;
    private int selectedCellCost = 0;

    private List<GameObject> activeCells = new List<GameObject>();
    private List<GameObject> activePathogens = new List<GameObject>();
    private bool levelFinished = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        CurrentDNA = startingDNA;

        Debug.Log($"[GameManager] Starting DNA: {CurrentDNA}");

        // ✅ Ensure result panels are hidden on game start
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (defeatPanel != null) defeatPanel.SetActive(false);
    }

    private void Update()
    {
        if (levelFinished)
            return;

        // ✅ Defeat: No DNA + No cells + Still pathogens
        if (CurrentDNA <= 0 && activeCells.Count == 0 && activePathogens.Count > 0)
        {
            Debug.Log("[GameManager] No DNA and no cells left — Defeat triggered.");
            TriggerDefeat();
        }

        if (selectedCellPrefab == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 spawnPos = new Vector2(worldPos.x, worldPos.y);

            TrySpawnSelectedCell(spawnPos);
        }
    }

    public void SelectCellToSpawn(GameObject prefab, int cost)
    {
        if (levelFinished)
            return;

        selectedCellPrefab = prefab;
        selectedCellCost = cost;
        Debug.Log($"[GameManager] Selected '{prefab.name}' for spawning. Cost: {cost} DNA. Remaining: {CurrentDNA}");
    }

    private void TrySpawnSelectedCell(Vector2 position)
    {
        if (selectedCellPrefab == null)
            return;

        if (activeCells.Count >= maxCellsOnField)
        {
            Debug.LogWarning($"[GameManager] Max cells ({maxCellsOnField}) reached.");
            return;
        }

        if (CurrentDNA < selectedCellCost)
        {
            Debug.LogWarning($"[GameManager] Not enough DNA ({CurrentDNA}) to spawn '{selectedCellPrefab.name}'.");
            return;
        }

        CurrentDNA -= selectedCellCost;

        GameObject newCell = Instantiate(selectedCellPrefab, position, Quaternion.identity);
        if (cellsParent != null)
            newCell.transform.SetParent(cellsParent);

        activeCells.Add(newCell);

        SpriteAttributes cellAttr = newCell.GetComponent<SpriteAttributes>();
        int cytokinesisCost = (cellAttr != null) ? cellAttr.cytokinesisValue : 0;
        CurrentStormValue += cytokinesisCost;

        Debug.Log($"[GameManager] Storm increased by {cytokinesisCost}. Now: {CurrentStormValue}/{stormThreshold}");

        OnStormValueChanged();
    }

    public void OnCellDeath(GameObject cellGO)
    {
        if (activeCells.Contains(cellGO))
        {
            activeCells.Remove(cellGO);
            Debug.Log($"[GameManager] A cell died. {activeCells.Count} remain.");

            SpriteAttributes attr = cellGO.GetComponent<SpriteAttributes>();
            if (attr != null)
            {
                CurrentStormValue -= attr.cytokinesisValue;
                CurrentStormValue = Mathf.Max(0, CurrentStormValue);

                Debug.Log($"[GameManager] Storm reduced by {attr.cytokinesisValue}. Now: {CurrentStormValue}/{stormThreshold}");

                OnStormValueChanged();
            }
        }
    }

    public void RegisterPathogen(GameObject pathogenGO)
    {
        if (!activePathogens.Contains(pathogenGO))
            activePathogens.Add(pathogenGO);
    }

    public void OnPathogenDeath(GameObject pathogenGO)
    {
        if (activePathogens.Contains(pathogenGO))
        {
            activePathogens.Remove(pathogenGO);
            Debug.Log($"[GameManager] A pathogen died. {activePathogens.Count} left.");

            if (activePathogens.Count == 0)
            {
                TriggerVictory();
            }
        }
    }

    private void OnStormValueChanged()
    {
        CytokinesisBar.Instance?.RefreshBar();

        // ✅ Properly trigger defeat when threshold is hit
        if (CurrentStormValue >= stormThreshold && !levelFinished)
        {
            Debug.Log("[GameManager] Storm threshold reached!");
            TriggerDefeat();
        }
    }

    private void TriggerVictory()
    {
        if (levelFinished) return;
        levelFinished = true;

        Debug.Log("[GameManager] VICTORY!");
        if (victoryPanel != null) victoryPanel.SetActive(true);
    }

    private void TriggerDefeat()
    {
        if (levelFinished) return;
        levelFinished = true;

        Debug.Log("[GameManager] DEFEAT!");
        if (defeatPanel != null) defeatPanel.SetActive(true);
    }

    public float GetStormNormalized()
    {
        if (stormThreshold <= 0) return 0f;
        return Mathf.Clamp01((float)CurrentStormValue / stormThreshold);
    }

    public int GetCurrentDNA() => CurrentDNA;
}
