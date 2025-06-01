using UnityEngine;

public class GameInitializer : MonoBehaviour
{
	void Start()
	{
		var fsm = GetComponent<FSMManager>();
		fsm.HandlePathogen(FSMManager.PathogenType.Bacteria);
	}
}
