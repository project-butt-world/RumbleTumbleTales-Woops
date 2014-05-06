#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#else
using UnityEngine;
#endif


public class FleeState : BaseState
{
	public float startFleeDistance = 5.0f;

	public FleeMode fleeMode = FleeMode.NearestTaggedObject;
	public string fleeTargetTag = "Untagged";
	public Transform fleeToTarget;
	public Vector3 fleeDirection;
	private Transform currentTarget;

	public BaseState fleeTargetReachedState;
	public float distanceToTargetThreshold = 1.0f;
	private float sqrDistanceToTargetThreshold = 1.0f;

	private GameObject[] fleeToObjects = null;


	public enum FleeMode
	{
		NearestTaggedObject,
		FixedTarget
	}


	protected override void Init(AIBehaviors fsm)
	{
		sqrDistanceToTargetThreshold = distanceToTargetThreshold * distanceToTargetThreshold;
		fsm.PlayAudio();
		fleeToObjects = GameObject.FindGameObjectsWithTag(fleeTargetTag);
	}

	protected override void StateEnded(AIBehaviors fsm)
	{
	}

	protected override bool Reason(AIBehaviors fsm)
	{
		if ( currentTarget != null )
		{
			float sqrDist = (fsm.transform.position - currentTarget.position).sqrMagnitude;

			if ( sqrDist < sqrDistanceToTargetThreshold )
			{
				fsm.ChangeActiveState(fleeTargetReachedState);
				return false;
			}
		}

		return true;
	}

	protected override void Action(AIBehaviors fsm)
	{
		switch ( fleeMode )
		{
//		case FleeMode.Direction:
//			break;
		case FleeMode.FixedTarget:
			if ( fleeToTarget != null )
			{
				currentTarget = fleeToTarget;
				fsm.MoveAgent(fleeToTarget, movementSpeed, rotationSpeed);
			}
			else
			{
				Debug.LogWarning("Flee To Target isn't set for FleeState");
			}

			break;
		case FleeMode.NearestTaggedObject:
			float nearestSqrDistance = Mathf.Infinity;
			int targetIndex = -1;

			for ( int i = 0; i < fleeToObjects.Length; i++ )
			{
				Vector3 dist = fleeToObjects[i].transform.position - this.transform.position;

				if ( dist.sqrMagnitude < nearestSqrDistance )
				{
					nearestSqrDistance = dist.sqrMagnitude;
					targetIndex = i;
				}
			}

			if ( targetIndex != -1 )
			{
				currentTarget = fleeToObjects[targetIndex].transform;
				fsm.MoveAgent(currentTarget, movementSpeed, rotationSpeed);
			}

			break;
		}
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

		m_State.Update();

		m_property = m_State.FindProperty("startFleeDistance");
		EditorGUILayout.PropertyField(m_property);

		EditorGUILayout.Separator();
		GUILayout.Label("Flee to target:", EditorStyles.boldLabel);

		m_property = m_State.FindProperty("fleeMode");
		EditorGUILayout.PropertyField(m_property);

		FleeMode fleeMode = (FleeMode)m_property.enumValueIndex;

		switch ( fleeMode )
		{
		case FleeMode.NearestTaggedObject:
			EditorGUILayout.Separator();
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("Use nearest object with tag:");

				m_property = m_State.FindProperty("fleeTargetTag");
				m_property.stringValue = EditorGUILayout.TagField(m_property.stringValue);
			}
			GUILayout.EndHorizontal();

			break;

		case FleeMode.FixedTarget:
			m_property = m_State.FindProperty("fleeToTarget");
			EditorGUILayout.PropertyField(m_property);
			break;
		}

		EditorGUILayout.Separator();
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("Flee Target Reached Transition:");
			m_property = m_State.FindProperty("fleeTargetReachedState");
			m_property.objectReferenceValue = AIBehaviorsStatePopups.DrawEnabledStatePopup(stateMachine, m_property.objectReferenceValue as BaseState);
		}
		GUILayout.EndHorizontal();

		m_property = m_State.FindProperty("distanceToTargetThreshold");
		float prevValue = m_property.floatValue;
		EditorGUILayout.PropertyField(m_property);

		if ( m_property.floatValue <= 0.0f )
			m_property.floatValue = prevValue;

		m_State.ApplyModifiedProperties();
	}
#endif
}