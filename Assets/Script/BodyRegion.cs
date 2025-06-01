using UnityEngine;

public class BodyRegion : MonoBehaviour
{
    public string regionName;
    public FSM fsm = new();
    public string currentState = "Resting";
    public CellType? activeCell = null;

    public void SetActiveCell(CellType cell)
    {
        activeCell = cell;
    }

    public void HandlePathogen(PathogenType pathogen)
    {
        if (activeCell == null) return;

        string input = pathogen.ToString();
        string state = currentState;

        FSMTransition transition = fsm.GetTransition(state, input);

        if (transition != null)
        {
            currentState = transition.NextState;
            Debug.Log($"{regionName}: Cleared {input} ? transitioned to {currentState} (speed: {transition.TransitionWeight})");
            // TODO: Spawn memory cell, play animation
        }
        else
        {
            currentState = "Dead";
            Debug.Log($"{regionName}: Failed to defend, pathogen spread!");
            // TODO: Trigger infection visuals, reduce health
        }
    }
}
