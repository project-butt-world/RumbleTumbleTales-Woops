using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class GoToRandomStateTrigger : BaseTrigger
{
	public BaseState[] states;
	public float[] weights;


	protected override void Init()
	{
	}


	protected override bool Evaluate(AIBehaviors fsm)
	{
		BaseState result = null;

		if ( GetRandomResult(fsm, out result, Random.value) )
		{
			fsm.ChangeActiveState(result);
			return true;
		}

		return false;
	}


	public bool GetRandomResult(AIBehaviors fsm, out BaseState resultState, float randomValue)
	{
		resultState = null;

		if ( states.Length > 0 )
		{
			float totalWeight = 1.0f;

			for ( int i = 0; i < states.Length; i++ )
			{
				if ( randomValue > totalWeight - weights[i] )
				{
					resultState = states[i];
					return true;
				}

				totalWeight -= weights[i];
			}

			resultState = states[states.Length - 1];
			return true;
		}
		else
		{
			Debug.LogWarning("The 'Go To Random State Trigger' requires at least 1 possible state!");
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