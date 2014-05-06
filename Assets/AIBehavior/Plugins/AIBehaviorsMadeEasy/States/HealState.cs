#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#else
using UnityEngine;
#endif


public class HealState : SeekState
{
	void Awake()
	{
		if ( enabled )
		{
			Debug.LogWarning("Please click on '" + transform.parent.name + "' in order to upgrade it as it uses the legacy state 'HealState' class.");
		}
	}


	protected override void Init(AIBehaviors fsm)
	{
		base.Init(fsm);
		fsm.PlayAudio();
	}


	protected override void StateEnded(AIBehaviors fsm)
	{
		base.StateEnded(fsm);
	}


	protected override bool Reason(AIBehaviors fsm)
	{
		return base.Reason(fsm);
	}


	protected override void Action(AIBehaviors fsm)
	{
		base.Action(fsm);
	}


#if UNITY_EDITOR
	// === Editor Methods === //

	public override void OnStateInspectorEnabled(SerializedObject m_ParentObject)
	{
		UpgradeLegacyDerivedState(m_ParentObject);
		base.OnStateInspectorEnabled(m_ParentObject);
	}


	protected override void DrawStateInspectorEditor(SerializedObject m_Object, AIBehaviors stateMachine)
	{
		base.DrawStateInspectorEditor(m_Object, stateMachine);
	}
#endif
}