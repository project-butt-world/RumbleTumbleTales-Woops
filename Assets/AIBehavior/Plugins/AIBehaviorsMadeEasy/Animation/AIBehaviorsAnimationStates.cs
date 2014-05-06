using UnityEngine;
using System.Collections.Generic;


public class AIBehaviorsAnimationStates : MonoBehaviour
{
	public bool is3D = true;
	public AIBehaviorsAnimationState[] states = new AIBehaviorsAnimationState[1];

	public GameObject animationStatesGameObject = null;

	private Dictionary<string, AIBehaviorsAnimationState> statesDictionary = new Dictionary<string, AIBehaviorsAnimationState>();


	public AIBehaviorsAnimationState GetStateWithName(string stateName)
	{
		if ( statesDictionary.ContainsKey(stateName) )
		{
			return statesDictionary[stateName];
		}

		for ( int i = 0; i < states.Length; i++ )
		{
			if ( states[i].name == stateName )
			{
				statesDictionary[stateName] = states[i];
				return states[i];
			}
		}

		return null;
	}
}