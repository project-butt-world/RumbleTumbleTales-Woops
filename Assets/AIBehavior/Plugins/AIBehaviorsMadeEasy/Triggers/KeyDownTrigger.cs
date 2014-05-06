using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class KeyDownTrigger : BaseTrigger
{
	public KeyCode keycode = KeyCode.E;


	protected override void Init()
	{
	}


	protected override bool Evaluate(AIBehaviors fsm)
	{
		// Logic here, return true if the trigger was triggered
		if ( Input.GetKeyDown(keycode) )
		{
			fsm.ChangeActiveState(transitionState);
			return true;
		}

		return false;
	}


#if UNITY_EDITOR
	/*
	// Implement your own custom GUI here if you want to
	public override void DrawInspectorProperties(SerializedObject sObject)
	{
	}
	*/
#endif
}