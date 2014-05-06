#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;


public class AIBehaviorsTriggerGizmos
{
	public static void DrawGizmos(AIBehaviors fsm, int selectedState)
	{
		Transform transform = fsm.transform;
		Vector3 position = transform.position;
		Quaternion rotation = transform.rotation;

		foreach ( BaseTrigger trigger in fsm.GetStateByIndex(selectedState).triggers )
		{
			Undo.SetSnapshotTarget(trigger, "Changes to Trigger");

			trigger.DrawGizmos(position, rotation);

			if ( GUI.changed )
			{
				EditorUtility.SetDirty(fsm.GetStateByIndex(selectedState));
			}
		}
	}
}
#endif