#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class AIBehaviorsTriggersGUI
{
	protected const string kArraySize = "triggers.Array.size";
	protected const string kArrayData = "triggers";

	static bool foldoutValue = false;
	static bool gotFoldoutValue = false;


	public static void Draw(SerializedObject m_Object, AIBehaviors fsm)
	{
		SerializedProperty m_Prop;
		bool newFoldoutValue;
		BaseState state = m_Object.targetObject as BaseState;

		EditorGUILayout.Separator();

		const string foldoutValueKey = "AIBehaviors_TriggersFoldout";

		if ( !gotFoldoutValue )
		{
			if ( EditorPrefs.HasKey(foldoutValueKey) )
				foldoutValue = EditorPrefs.GetBool(foldoutValueKey);

			gotFoldoutValue = true;
		}

		newFoldoutValue = foldoutValue;
		newFoldoutValue = EditorGUILayout.Foldout(foldoutValue, "Triggers:", EditorStyles.foldoutPreDrop);

		if ( foldoutValue != newFoldoutValue )
		{
			foldoutValue = newFoldoutValue;
			EditorPrefs.SetBool(foldoutValueKey, foldoutValue);
		}

		if ( !foldoutValue )
		{
			return;
		}

		string[] triggerTypeNames = AIBehaviorsComponentInfoHelper.GetTriggerTypeNames();

		foreach ( string triggerType in triggerTypeNames )
		{
			bool hasThisState = false;
			bool toggleValue = false;
			BaseTrigger currentTrigger = null;
			int currentTriggerIndex = 0;

			for ( int j = 0; j < state.triggers.Length; j++ )
			{
				BaseTrigger trigger = state.triggers[j];

				if ( trigger.GetType().ToString() == triggerType )
				{
					hasThisState = true;
					currentTrigger = trigger;
					currentTriggerIndex = j;

					break;
				}
			}

			toggleValue = GUILayout.Toggle(hasThisState, AIBehaviorsComponentInfoHelper.GetNameFromType(triggerType));

			if ( hasThisState != toggleValue )
			{
				Undo.RegisterSceneUndo("Modified a triggers existence");

				if ( hasThisState )
				{
					Component.DestroyImmediate(currentTrigger);

					m_Prop = m_Object.FindProperty(kArrayData);

					AIBehaviorsAssignableObjectArray.RemoveObjectAtIndex(m_Object, currentTriggerIndex, kArrayData);
				}
				else
				{
					Component newTrigger = fsm.statesGameObject.AddComponent(triggerType);
					int lastIndex;

					m_Prop = m_Object.FindProperty(kArraySize);
					lastIndex = m_Prop.intValue;
					m_Prop.intValue++;

					m_Prop = m_Object.FindProperty(kArrayData);
					m_Prop = m_Prop.GetArrayElementAtIndex(lastIndex);
					m_Prop.objectReferenceValue = newTrigger;
				}

				continue;
			}

			if ( toggleValue )
			{
				currentTrigger.DrawInspectorGUI(fsm);
				EditorGUILayout.Separator();
			}
		}

		m_Object.ApplyModifiedProperties();
	}
}
#endif