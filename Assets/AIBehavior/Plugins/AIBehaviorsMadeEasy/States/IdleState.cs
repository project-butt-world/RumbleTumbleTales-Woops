#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#else
using UnityEngine;
#endif


public class IdleState : BaseState
{
	private GameObject targetRotationObject = null;


	protected override void Init(AIBehaviors fsm)
	{
		Transform fsmTFM = fsm.transform;
		Transform targetRotationTFM;

		targetRotationObject = new GameObject("RotationTarget");
		targetRotationTFM = targetRotationObject.transform;
		targetRotationTFM.position = fsmTFM.position + fsmTFM.forward;
		fsm.RotateAgent(targetRotationTFM, rotationSpeed);

		targetRotationTFM.parent = fsmTFM;

		fsm.PlayAudio();
	}


	protected override void StateEnded(AIBehaviors fsm)
	{
		Destroy (targetRotationObject);
	}


	protected override bool Reason(AIBehaviors fsm)
	{
		return true;
	}


	protected override void Action(AIBehaviors fsm)
	{
		fsm.MoveAgent(fsm.transform, 0.0f, rotationSpeed);
	}


#if UNITY_EDITOR
	// === Editor Methods === //

	public override void OnStateInspectorEnabled(SerializedObject m_ParentObject)
	{
	}


	protected override void DrawStateInspectorEditor(SerializedObject m_Object, AIBehaviors stateMachine)
	{
	}
#endif
}