using UnityEngine;
using System.Collections.Generic;

public class FSMManager : MonoBehaviour
{
    public enum PathogenType { Bacteria, Virus, Tumor, Toxin }
    public enum CellType { None, Macrophage, TCell, BCell, NKCell }

    [System.Serializable]
    public class FSMTransition
    {
        public CellType cell;
        public PathogenType pathogen;
        public string nextRegion;
    }

    public List<FSMTransition> transitions;
    public string currentRegion = "Region_Arm"; // for testing

    public void HandlePathogen(PathogenType incoming)
    {
        foreach (var t in transitions)
        {
            if (t.cell == GetActiveCell(currentRegion) && t.pathogen == incoming)
            {
                Debug.Log($"Pathogen destroyed! Moving to {t.nextRegion}");
                currentRegion = t.nextRegion;
                return;
            }
        }
        Debug.Log("Infection spread. Game Over?");
    }

    private CellType GetActiveCell(string region)
    {
        // Just a stub for testing — you’d link to drag/drop logic here
        return CellType.Macrophage;
    }
}
