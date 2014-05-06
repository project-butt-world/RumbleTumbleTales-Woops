#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#else
using UnityEngine;
#endif


public class HelpState : BaseState
{
	public float withinHelpPointDistance = 1.0f;
	public Vector3 helpPoint;
	public BaseState helpPointReachedState = null;

	public string[] messageFilter = new string[0];

	public float healthAmount;


	protected override void Init(AIBehaviors fsm)
	{
		fsm.PlayAudio();
	}


	protected override void StateEnded(AIBehaviors fsm)
	{
	}


	protected override bool Reason(AIBehaviors fsm)
	{
		if ( Vector3.Distance(fsm.transform.position, helpPoint) < withinHelpPointDistance )
		{
			if ( helpPointReachedState != null )
			{
				fsm.ChangeActiveState(helpPointReachedState);
				return false;
			}
		}

		return true;
	}


	protected override void Action(AIBehaviors fsm)
	{
		fsm.MoveAgent(helpPoint, movementSpeed, rotationSpeed);
	}


#if UNITY_EDITOR
	// === Editor Methods === //

	public override void OnStateInspectorEnabled(SerializedObject m_ParentObject)
	{
	}


	protected override void DrawStateInspectorEditor(SerializedObject m_Object, AIBehaviors stateMachine)
	{
		SerializedObject m_State = new SerializedObject(this);
		SerializedProperty m_Prop;

		m_Prop = m_State.FindProperty("withinHelpPointDistance");
		EditorGUILayout.PropertyField(m_Prop);

		EditorGUILayout.Separator();

		GUILayout.Label("State when help point reached");
		m_Prop = m_State.FindProperty("helpPointReachedState");
		m_Prop.objectReferenceValue = AIBehaviorsStatePopups.DrawEnabledStatePopup(stateMachine, m_Prop.objectReferenceValue as BaseState);

		m_State.ApplyModifiedProperties();
	}
#endif
}