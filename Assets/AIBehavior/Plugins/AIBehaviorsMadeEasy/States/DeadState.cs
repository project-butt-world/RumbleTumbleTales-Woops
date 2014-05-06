#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#else
using UnityEngine;
#endif


public class DeadState : BaseState
{
	public GameObject spawnPickup;

	public bool removeWhenDead = true;
	public float removeTime = 1.0f;

	public RemovalMode removalMode = RemovalMode.Flash;

	public int pointValue = 0;

	public float healthAmount = 0.0f;


	public enum RemovalMode
	{
		Flash,
		Fade,
		None
	}


	protected override void Init(AIBehaviors fsm)
	{
		fsm.PlayAudio();
		fsm.MoveAgent(fsm.transform.position, 0.0f, 0.0f);
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