#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#else
using UnityEngine;
#endif


public class PatrolState : BaseState
{
	Transform rotationHelper = null;

	public Transform patrolPointsGroup;
	private Transform[] patrolPoints;
	private int currentPatrolPoint = 0;
	private int patrolDirection = 1;

	public float pointDistanceThreshold = 1.0f;

	public PatrolMode patrolMode = PatrolMode.Loop;
	public ContinuePatrolMode continuePatrolMode = ContinuePatrolMode.NearestNextNode;
	public BaseState patrolEndedState;


	public enum PatrolMode
	{
		Once,
		Loop,
		PingPong
	}


	public enum ContinuePatrolMode
	{
		Reset,
		ContinuePrevious,
		NearestNode,
		NearestNextNode
	}


	public PatrolState()
	{
		patrolDirection = 1;
		currentPatrolPoint = 0;
	}


	void Awake()
	{
		if ( isEnabled )
		{
			if ( patrolPointsGroup != null )
			{
				GetSortedPatrolPoints();
			}
			else
			{
				if ( transform.parent != null )
				{
					Debug.LogWarning("The variable 'patrolPointsGroup' is unassigned for the 'Patrol' state on " + transform.parent.name);
				}
			}
		}
	}


	// === Public Methods === //

	public Transform GetCurrentPatrolTarget()
	{
		if ( currentPatrolPoint < 0 || currentPatrolPoint >= patrolPoints.Length )
		{
			return null;
		}

		return patrolPoints[currentPatrolPoint];
	}


	void GetSortedPatrolPoints()
	{
		Transform[] tfms = patrolPointsGroup.GetComponentsInChildren<Transform>();
		int curIndex = 0;

		patrolPoints = new Transform[tfms.Length - 1];

		for ( int i = 0; i < tfms.Length; i++ )
		{
			if ( tfms[i] != patrolPointsGroup )
			{
				patrolPoints[curIndex] = tfms[i];
				curIndex++;
			}
		}

		for ( int i = 0; i < patrolPoints.Length; i++ )
		{
			for ( int j = i+1; j < patrolPoints.Length; j++ )
			{
				if ( patrolPoints[i].name.CompareTo(patrolPoints[j].name) > 0 )
				{
					Transform temp = patrolPoints[i];

					patrolPoints[i] = patrolPoints[j];
					patrolPoints[j] = temp;
				}
			}
		}
	}


	protected override void Init(AIBehaviors fsm)
	{
		if ( rotationHelper == null )
		{
			rotationHelper = (new GameObject("RotationHelper")).transform;
			rotationHelper.parent = fsm.transform;
			rotationHelper.localPosition = Vector3.forward;
		}

		switch ( continuePatrolMode )
		{
			case ContinuePatrolMode.Reset:
				patrolDirection = 1;
				currentPatrolPoint = 0;
				break;

			case ContinuePatrolMode.NearestNode:
			case ContinuePatrolMode.NearestNextNode:
				Vector3 thisPos = fsm.transform.position;
				int nearestNode = 0;
				float nearestSqrMagnitude = Mathf.Infinity;

				for ( int i = 0; i < patrolPoints.Length; i++ )
				{
					float thisSqrMagnitude = (thisPos - patrolPoints[i].position).sqrMagnitude;

					if ( nearestSqrMagnitude > thisSqrMagnitude )
					{
						nearestSqrMagnitude = thisSqrMagnitude;
						nearestNode = i;
					}
				}

				if ( continuePatrolMode == ContinuePatrolMode.NearestNode )
					currentPatrolPoint = nearestNode;
				else if ( patrolPoints.Length != 0 )
					currentPatrolPoint = (nearestNode + 1) % patrolPoints.Length;

				break;

			case ContinuePatrolMode.ContinuePrevious:
				break;
		}

		fsm.RotateAgent(rotationHelper, rotationSpeed);

		fsm.PlayAudio();
	}


	protected override void StateEnded(AIBehaviors fsm)
	{
	}


	protected override bool Reason(AIBehaviors fsm)
	{
		if ( patrolMode == PatrolMode.Once )
		{
			// Is the patrol ended?
			if ( currentPatrolPoint >= patrolPoints.Length )
			{
				currentPatrolPoint = 0;
				fsm.ChangeActiveState(patrolEndedState);

				return false;
			}
		}

		return true;
	}


	protected override void Action(AIBehaviors fsm)
	{
		Vector3 targetPoint = patrolPoints[currentPatrolPoint].position;
		Vector3 thisPos = fsm.transform.position;

		fsm.MoveAgent(targetPoint, movementSpeed, rotationSpeed);

		if ( (thisPos - targetPoint).sqrMagnitude < pointDistanceThreshold * pointDistanceThreshold )
		{
			currentPatrolPoint += patrolDirection;

			// Check for once, loop, ping pong, clamp forever

			if ( patrolMode == PatrolMode.Loop )
			{
				currentPatrolPoint %= patrolPoints.Length;
			}
			else if ( patrolMode == PatrolMode.PingPong )
			{
				if ( patrolDirection == 1 && currentPatrolPoint == patrolPoints.Length )
				{
					currentPatrolPoint = patrolPoints.Length - 1;
					patrolDirection = -1;
				}
				else if ( currentPatrolPoint == 0 )
				{
					currentPatrolPoint = 0;
					patrolDirection = 1;
				}
			}
		}
	}


#if UNITY_EDITOR
	// === Editor Functions === //

	public override void OnStateInspectorEnabled(SerializedObject m_ParentObject)
	{
	}


	protected override void DrawStateInspectorEditor(SerializedObject m_Object, AIBehaviors stateMachine)
	{
		SerializedObject m_State = new SerializedObject(this);
		SerializedProperty m_property;

		m_State.Update();

		// === Handle other properties === //

		EditorGUILayout.Separator();
		GUILayout.Label("Movement Options:", EditorStyles.boldLabel);

		PatrolMode patrolMode;

		m_property = m_State.FindProperty("patrolMode");
		EditorGUILayout.PropertyField(m_property);
		patrolMode = (PatrolMode)m_property.enumValueIndex;

		m_property = m_State.FindProperty("continuePatrolMode");
		EditorGUILayout.PropertyField(m_property, new GUIContent("Continue Previous Patrol", "When this state is switched back to, will it continue where it left off?"));

		if ( patrolMode == PatrolState.PatrolMode.Once )
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("End Patrol Transition:");
				m_property = m_State.FindProperty("patrolEndedState");
				m_property.objectReferenceValue = AIBehaviorsStatePopups.DrawEnabledStatePopup(stateMachine, m_property.objectReferenceValue as BaseState);
			}
			GUILayout.EndHorizontal();
		}

		// Handle the patrol points

		if ( patrolPoints == null )
			patrolPoints = new Transform[0];

		EditorGUILayout.Separator();
		GUILayout.Label("Patrol Points:", EditorStyles.boldLabel);

		m_property = m_State.FindProperty("patrolPointsGroup");
		EditorGUILayout.PropertyField(m_property);

		m_property = m_State.FindProperty("pointDistanceThreshold");
		EditorGUILayout.PropertyField(m_property, new GUIContent("Distance Threshold"));

		if ( m_property.floatValue < 0.0f )
			m_property.floatValue = 0.0f;

		m_State.ApplyModifiedProperties();
	}
#endif
}