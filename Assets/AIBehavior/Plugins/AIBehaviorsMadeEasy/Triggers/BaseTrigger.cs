using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif


public abstract class BaseTrigger : MonoBehaviour
{
	public BaseState transitionState;


	// === Trigger Methods === //

	protected abstract void Init();
	protected abstract bool Evaluate(AIBehaviors fsm);


	public void HandleInit()
	{
		Init();
	}


	public bool HandleEvaluate(AIBehaviors fsm)
	{
		if ( !this.enabled )
			return false;

		return Evaluate(fsm);
	}


#if UNITY_EDITOR
	public void DrawInspectorGUI(AIBehaviors fsm)
	{
		SerializedObject sObject = new SerializedObject(this);
		SerializedProperty prop;

		DrawInspectorProperties(sObject);

		GUILayout.BeginHorizontal();
		GUILayout.Label("Change to State:");
		prop = sObject.FindProperty("transitionState");
		prop.objectReferenceValue = AIBehaviorsStatePopups.DrawEnabledStatePopup(fsm, prop.objectReferenceValue as BaseState);
		GUILayout.EndHorizontal();

		sObject.ApplyModifiedProperties();
	}


	public virtual void DrawInspectorProperties(SerializedObject sObject)
	{
		InspectorHelper.DrawInspector(sObject);
	}


	public virtual void DrawGizmos(Vector3 position, Quaternion rotation)
	{
	}
#endif
}