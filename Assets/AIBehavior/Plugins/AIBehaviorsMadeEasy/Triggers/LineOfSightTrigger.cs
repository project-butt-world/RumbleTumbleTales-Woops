using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class LineOfSightTrigger : BaseTrigger
{
	protected override void Init()
	{
	}


	protected override bool Evaluate(AIBehaviors fsm)
	{
		if ( fsm.GetClosestPlayerWithinSight(true) != null )
		{
			fsm.ChangeActiveState(transitionState);
			return true;
		}

		return false;
	}


#if UNITY_EDITOR
	public override void DrawInspectorProperties(SerializedObject sObject)
	{
	}
#endif
}