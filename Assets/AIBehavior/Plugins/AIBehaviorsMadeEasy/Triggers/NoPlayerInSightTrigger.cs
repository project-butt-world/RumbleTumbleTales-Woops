using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class NoPlayerInSightTrigger : BaseTrigger
{
	

	protected override void Init()
	{
	}


	protected override bool Evaluate(AIBehaviors fsm)
	{
		if ( fsm.GetClosestPlayerWithinSight(false) == null )
		{
			fsm.ChangeActiveState(transitionState);
			return true;
		}

		return false;
	}
}