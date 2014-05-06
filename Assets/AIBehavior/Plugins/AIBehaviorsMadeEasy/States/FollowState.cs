#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#else
using UnityEngine;
#endif


public class FollowState : BaseState
{
	public Transform targetToFollow;

	// Chase is a fast paced following
	public float chaseSpeed = 1.0f;
	public float startChaseDistance = 10.0f;
	public float breakChaseDistance = 20.0f;
	public float chaseDuration = 10.0f;

	public bool horizontalMove = true;
	public bool verticalMove = false;

	public bool stopWhenTargetReached = true;
	public float passingDistance = 10.0f;

//	public bool findNewPath = false;

	Transform lastSightedPlayer;
	Vector3 lastSightedPlayerLocation;

	enum FollowMode
	{
		Normal,
		Chase
	}
	FollowMode curFollowMode = FollowMode.Normal;


	protected override void Init(AIBehaviors fsm)
	{
		fsm.PlayAudio();
	}


	protected override void StateEnded(AIBehaviors fsm)
	{
	}


	protected override bool Reason(AIBehaviors fsm)
	{
		Transform sightedPlayer;
		float closestPlayerDistance = Mathf.Infinity;

		sightedPlayer = fsm.GetClosestPlayerWithinSight(out closestPlayerDistance, false);
		if ( sightedPlayer != null )
		{
			float sqrStartChaseDistance = startChaseDistance * startChaseDistance;
			float sqrBreakChaseDistance = breakChaseDistance * breakChaseDistance;

			if ( closestPlayerDistance > sqrBreakChaseDistance )
			{
				curFollowMode = FollowMode.Normal;
			}
			else if ( closestPlayerDistance < sqrStartChaseDistance )
			{
				curFollowMode = FollowMode.Chase;
			}

			lastSightedPlayer = sightedPlayer;
			lastSightedPlayerLocation = lastSightedPlayer.position;
		}
		else
			lastSightedPlayer = null;

		return true;
	}


	protected override void Action(AIBehaviors fsm)
	{
		float speed = 0.0f;

		if ( curFollowMode == FollowMode.Chase )
			speed = chaseSpeed;
		else
			speed = movementSpeed;

		if ( lastSightedPlayer != null )
			fsm.MoveAgent(lastSightedPlayer, speed, rotationSpeed);
		else
			fsm.MoveAgent(lastSightedPlayerLocation, speed, rotationSpeed);
	}


#if UNITY_EDITOR
	// === Editor Methods === //

	public override void OnStateInspectorEnabled(SerializedObject m_ParentObject)
	{
	}


	protected override void DrawStateInspectorEditor(SerializedObject m_Object, AIBehaviors stateMachine)
	{
		SerializedObject m_State = new SerializedObject(this);
		SerializedProperty m_property;

		// === Chase Properties === //

		GUILayout.Label("Chase Properties:", EditorStyles.boldLabel);

		m_property = m_State.FindProperty("chaseSpeed");
		EditorGUILayout.PropertyField(m_property);

		m_property = m_State.FindProperty("startChaseDistance");
		if ( m_property.floatValue > this.breakChaseDistance )
			AIBehaviorsGeneralEditorGUI.DrawWarning("Start Chase Distance should be less than Break Chase Distance");
		EditorGUILayout.PropertyField(m_property);

		m_property = m_State.FindProperty("breakChaseDistance");
		EditorGUILayout.PropertyField(m_property);

		m_property = m_State.FindProperty("chaseDuration");
		EditorGUILayout.PropertyField(m_property);

		// === Move Directions === //

		EditorGUILayout.Separator();
		GUILayout.Label("Move Directions:", EditorStyles.boldLabel);

		m_property = m_State.FindProperty("horizontalMove");
		EditorGUILayout.PropertyField(m_property);

		m_property = m_State.FindProperty("verticalMove");
		EditorGUILayout.PropertyField(m_property);

		// === Other Properties === //

		EditorGUILayout.Separator();
		GUILayout.Label("Other Properties:", EditorStyles.boldLabel);

		m_property = m_State.FindProperty("stopWhenTargetReached");
		EditorGUILayout.PropertyField(m_property);

		m_property = m_State.FindProperty("passingDistance");
		EditorGUILayout.PropertyField(m_property);

		m_State.ApplyModifiedProperties();
	}
#endif
}