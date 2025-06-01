using UnityEngine;

public class StartTest : MonoBehaviour
{
    private void Start()
    {
        var fsm = GetComponent<FSMManager>();
        fsm.HandlePathogen(FSMManager.PathogenType.Bacteria);
    }
}
