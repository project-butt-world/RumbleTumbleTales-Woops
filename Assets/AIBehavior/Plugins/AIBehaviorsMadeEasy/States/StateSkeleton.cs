#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#else
using UnityEngine;
#endif


public class StateSkeleton : BaseState
{
	protected override void Init(AIBehaviors fsm)
	{
	}


	protected override void StateEnded(AIBehaviors fsm)
	{
	}


	protected override bool Reason(AIBehaviors fsm)
	{
		return true;
	}


	protected override void Action(AIBehaviors fsm)
	{
	}


#if UNITY_EDITOR
	// === Editor Methods === //

	public override void OnStateInspectorEnabled(SerializedObject m_ParentObject)
	{
	}


	protected override void DrawStateInspectorEditor(SerializedObject m_Object, AIBehaviors stateMachine)
	{
		InspectorHelper.DrawInspector(m_Object);
		m_Object.ApplyModifiedProperties();
	}
#endif
}