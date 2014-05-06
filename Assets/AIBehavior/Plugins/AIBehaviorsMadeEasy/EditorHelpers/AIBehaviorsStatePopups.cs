#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

using System.Collections.Generic;


public class AIBehaviorsStatePopups
{
	public static BaseState DrawEnabledStatePopup(AIBehaviors fsm, BaseState curState)
	{
		List<string> statesList = new List<string>();
		Dictionary<string, BaseState> statesDictionary = new Dictionary<string, BaseState>();
		BaseState[] states = fsm.GetAllStates();
		string[] stateSelections;
		int selection = 0;

		// Get the state names
		for ( int i = 0; i < fsm.stateCount; i++ )
		{
			// Should we include state i in the list?
			if ( states[i].isEnabled )
			{
				string stateName = states[i].name;

				statesList.Add(stateName);
				statesDictionary[stateName] = states[i];

				if ( states[i] == curState )
				{
					selection = statesList.Count-1;
				}
			}
		}
		stateSelections = statesList.ToArray();

		selection = EditorGUILayout.Popup(selection, stateSelections);

		return statesDictionary[stateSelections[selection]];
	}
}
#endif