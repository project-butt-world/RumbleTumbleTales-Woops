using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class LowHealthTrigger : BaseTrigger
{
	public float healthThreshold = 50.0f;
	

	protected override void Init()
	{
	}


	protected override bool Evaluate(AIBehaviors fsm)
	{
		if ( IsThresholdCrossed(fsm) )
		{
			fsm.ChangeActiveState(transitionState);
			return true;
		}

		return false;
	}


	public bool IsThresholdCrossed(AIBehaviors fsm)
	{
		return fsm.GetHealthValue() < healthThreshold;
	}


#if UNITY_EDITOR
	public override void DrawInspectorProperties(SerializedObject sObject)
	{
		SerializedProperty prop = sObject.FindProperty("healthThreshold");
		EditorGUILayout.PropertyField(prop, new GUIContent("Below Health"));
	}
#endif
}