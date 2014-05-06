#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#else
using UnityEngine;
#endif


public class GotHitState : BaseState
{
	public bool hitMovesPosition = true;
	public float movePositionAmount = 1.0f;


	protected override void Init(AIBehaviors fsm)
	{
		fsm.PlayAudio();
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
		SerializedObject m_State = new SerializedObject(this);

		m_State.ApplyModifiedProperties();
	}
#endif
}