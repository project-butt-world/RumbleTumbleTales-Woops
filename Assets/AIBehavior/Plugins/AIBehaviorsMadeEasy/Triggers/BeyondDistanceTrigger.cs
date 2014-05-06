using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class BeyondDistanceTrigger : BaseTrigger
{
	public float distanceThreshold = 1.0f;


	protected override void Init()
	{
	}


	protected override bool Evaluate(AIBehaviors fsm)
	{
		Transform[] tfms = fsm.GetPlayerTransforms();
		Vector3 thisTFMPos = fsm.transform.position;

		for ( int i = 0; i < tfms.Length; i++ )
		{
			Vector3 targetDir = tfms[i].position - thisTFMPos;

			if ( targetDir.sqrMagnitude > distanceThreshold * distanceThreshold )
			{
				fsm.ChangeActiveState(transitionState);
				return true;
			}
		}

		return false;
	}


#if UNITY_EDITOR
	public override void DrawInspectorProperties(SerializedObject sObject)
	{
		SerializedProperty prop = sObject.FindProperty("distanceThreshold");
		EditorGUILayout.PropertyField(prop, new GUIContent("Distance"));
	}


	public override void DrawGizmos(Vector3 position, Quaternion rotation)
	{
		Handles.color = Color.blue;

		distanceThreshold = Handles.RadiusHandle(rotation, position, distanceThreshold);
	}
#endif
}