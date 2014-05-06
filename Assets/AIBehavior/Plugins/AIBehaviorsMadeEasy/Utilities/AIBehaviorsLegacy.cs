using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class AIBehaviorsLegacy
{
	public static void UpgradeTriggers(AIBehaviors fsm)
	{
		bool upgradedTriggers = false;
		BaseState[] states = fsm.GetAllStates();

		foreach ( BaseState state in states )
		{
			List<BaseTrigger> newTriggers = new List<BaseTrigger>();
			
#if UNITY_EDITOR
			GameObject triggersGameObject;
			Transform fsmTransform;
			SerializedObject obj = new SerializedObject(state);
			SerializedProperty prop = null;

			if ( state.isUpgraded )
				continue;

			if ( Application.isPlaying )
			{
				triggersGameObject = fsm.statesGameObject;
				fsmTransform = fsm.transform;
			}
			else if ( Selection.activeTransform != null )
			{
				fsmTransform = Selection.activeTransform;
				triggersGameObject = fsmTransform.gameObject;
			}
			else
			{
				return;
			}

			foreach ( Transform tfm in fsmTransform )
			{
				if ( tfm.name == "States" )
				{
					if (tfm.gameObject.GetInstanceID() == fsm.statesGameObject.GetInstanceID() )
					{
						triggersGameObject = tfm.gameObject;
					}
				}
			}
#else
			GameObject triggersGameObject = fsm.statesGameObject;
#endif

			if ( state.usesLineOfSightTrigger )
			{
				LineOfSightTrigger trigger = triggersGameObject.AddComponent<LineOfSightTrigger>();

				trigger.transitionState = state.lineOfSightState;

				newTriggers.Add(trigger);
			}

			if ( state.usesNoPlayerInSightTrigger )
			{
				NoPlayerInSightTrigger trigger = triggersGameObject.AddComponent<NoPlayerInSightTrigger>();

				trigger.transitionState = state.noPlayerInSightState;

				newTriggers.Add(trigger);
			}

			if ( state.usesWithinDistanceTrigger )
			{
				WithinDistanceTrigger trigger = triggersGameObject.AddComponent<WithinDistanceTrigger>();

				trigger.distanceThreshold = state.withinDistance;
				trigger.transitionState = state.withinDistanceState;

				state.usesWithinDistanceTrigger = false;

				newTriggers.Add(trigger);
			}

			if ( state.usesBeyondDistanceTrigger )
			{
				BeyondDistanceTrigger trigger = triggersGameObject.AddComponent<BeyondDistanceTrigger>();

				trigger.distanceThreshold = state.beyondDistance;
				trigger.transitionState = state.beyondDistanceState;

				newTriggers.Add(trigger);
			}

			if ( state.usesTimerTrigger )
			{
				TimerTrigger trigger = triggersGameObject.AddComponent<TimerTrigger>();

				trigger.duration = state.timerDuration;
				trigger.transitionState = state.timerExpiredState;

				newTriggers.Add(trigger);
			}

			if ( state.usesLowHealthTrigger )
			{
				LowHealthTrigger trigger = triggersGameObject.AddComponent<LowHealthTrigger>();

				trigger.healthThreshold = state.lowHealthAmount;
				trigger.transitionState = state.lowHealthState;

				newTriggers.Add(trigger);
			}

			if ( state.usesHighHealthTrigger )
			{
				HighHealthTrigger trigger = triggersGameObject.AddComponent<HighHealthTrigger>();

				trigger.healthThreshold = state.highHealthAmount;
				trigger.transitionState = state.highHealthState;

				newTriggers.Add(trigger);
			}

			if ( state.usesInPlayerViewTrigger )
			{
				InPlayerViewTrigger trigger = triggersGameObject.AddComponent<InPlayerViewTrigger>();

				trigger.transitionState = state.inPlayerViewState;

				newTriggers.Add(trigger);
			}

			if ( newTriggers.Count > 0 )
			{
				upgradedTriggers = true;

#if UNITY_EDITOR
				prop = obj.FindProperty("triggers");
				prop.arraySize = newTriggers.Count;

				for ( int i = 0; i < newTriggers.Count; i++ )
				{
					const string kArrayData = "triggers.Array.data[{0}]";

					prop = obj.FindProperty(string.Format(kArrayData, i));
					prop.objectReferenceValue = newTriggers[i];

					Debug.Log(state.GetType().ToString() + " : " + newTriggers[i].GetType().ToString());
				}

				obj.FindProperty("usesLineOfSightTrigger").boolValue = false;
				obj.FindProperty("usesNoPlayerInSightTrigger").boolValue = false;
				obj.FindProperty("usesWithinDistanceTrigger").boolValue = false;
				obj.FindProperty("usesBeyondDistanceTrigger").boolValue = false;
				obj.FindProperty("usesTimerTrigger").boolValue = false;
				obj.FindProperty("usesLowHealthTrigger").boolValue = false;
				obj.FindProperty("usesHighHealthTrigger").boolValue = false;
				obj.FindProperty("usesInPlayerViewTrigger").boolValue = false;

				obj.ApplyModifiedProperties();
#else
				state.triggers = newTriggers.ToArray();

				state.usesLineOfSightTrigger = false;
				state.usesNoPlayerInSightTrigger = false;
				state.usesWithinDistanceTrigger = false;
				state.usesBeyondDistanceTrigger = false;
				state.usesTimerTrigger = false;
				state.usesLowHealthTrigger = false;
				state.usesHighHealthTrigger = false;
#endif
			}
		}

		if ( Application.isPlaying && upgradedTriggers )
		{
			Debug.LogWarning("Please click on the '" + fsm.name + "' AIBehaviors object in edit mode to upgrade to the new triggers system.");
		}
		else if ( upgradedTriggers )
		{
			Debug.LogWarning("Upgraded '" + fsm.name + "'. Some things may have broken in the process.");
		}
	}
}