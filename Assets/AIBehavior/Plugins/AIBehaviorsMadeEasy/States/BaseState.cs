#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#else
using UnityEngine;
#endif

using Random = UnityEngine.Random;


public abstract class BaseState : MonoBehaviour
{
	public bool isEnabled = true;

	new public string name = "";

	// === Triggers === //

	public BaseTrigger[] triggers = new BaseTrigger[0];

	// === Cooldown Properties === //

	public float cooldownTime = 0.0f;
	private float cooledDownTime = 0.0f;
	public bool hasCooldownLimit = false;
	public int cooldownLimit = 3;
	public BaseState cooldownLimitExceededState;
	private int cooldowns = 0;

	// === General Variables === //

	public float movementSpeed = 1.0f;
	public float rotationSpeed = 90.0f;
	private float lastActionTime = 0.0f;
	protected float deltaTime = 0.0f;

	// === Animation Variables === //

	public AIBehaviorsAnimationStates animationStatesComponent;
	public AIBehaviorsAnimationState[] animationStates = new AIBehaviorsAnimationState[1];

	// === Audio Variables === //

	public AudioClip audioClip = null;
	public float audioVolume = 1.0f;
	public bool loopAudio = false;


	// === State Methods === //

	protected abstract void Init(AIBehaviors fsm);
	protected abstract bool Reason(AIBehaviors fsm);
	protected abstract void Action(AIBehaviors fsm);
	protected abstract void StateEnded(AIBehaviors fsm);


	// === Init === //

	public void InitState(AIBehaviors fsm)
	{
		cooldowns = 0;
		cooledDownTime = 0.0f;
		lastActionTime = Time.time;
		deltaTime = 0.0f;

		InitTriggers();
		Init(fsm);
		PlayRandomAnimation(fsm);
	}


	// === EndState === //

	public void EndState(AIBehaviors fsm)
	{
		StateEnded(fsm);
	}


	private void InitTriggers()
	{
		foreach ( BaseTrigger trigger in triggers )
		{
			trigger.HandleInit();
		}
	}


	// === Reason === //
	// Returns true if the state remained the same

	public bool HandleReason(AIBehaviors fsm)
	{
		if ( CheckTriggers(fsm) )
		{
			return false;
		}

		return Reason(fsm);
	}


	protected bool CheckTriggers(AIBehaviors fsm)
	{
		foreach ( BaseTrigger trigger in triggers )
		{
			if ( trigger.HandleEvaluate(fsm) )
			{
				return true;
			}
		}

		return false;
	}


	// === Action === //

	public void HandleAction(AIBehaviors fsm)
	{
		deltaTime = Time.time - lastActionTime;
		lastActionTime = Time.time;

		// Can we attack again ?
		if ( cooledDownTime < Time.time )
		{
			if ( hasCooldownLimit )
			{
				if ( cooldowns > cooldownLimit )
				{
					fsm.ChangeActiveState(cooldownLimitExceededState);
					return;
				}
				else
					cooldowns++;
			}

			cooledDownTime = Time.time + cooldownTime;

			Action(fsm);
		}
	}


	// === Animation === //

	public void PlayRandomAnimation(AIBehaviors fsm)
	{
		if ( animationStates.Length > 0 )
		{
			int randAnimState = (int)(Random.value * animationStates.Length);

			fsm.PlayAnimation(animationStates[randAnimState]);
		}
	}


	// === Deprecated Trigger Properties === //

	public bool isUpgraded = false;

	public bool usesLineOfSightTrigger = false;
	public BaseState lineOfSightState;

	public bool usesNoPlayerInSightTrigger = false;
	public BaseState noPlayerInSightState;

	public bool usesWithinDistanceTrigger = false;
	public BaseState withinDistanceState;
	public float withinDistance = 1.0f;

	public bool usesBeyondDistanceTrigger = false;
	public BaseState beyondDistanceState;
	public float beyondDistance = 1.0f;

	public bool usesTimerTrigger = false;
	public BaseState timerExpiredState;
	public float timerDuration = 1.0f;
	public float plusOrMinusDuration = 0.0f;

	public bool usesLowHealthTrigger = false;
	public float lowHealthAmount = 50.0f;
	public BaseState lowHealthState;

	public bool usesHighHealthTrigger = false;
	public float highHealthAmount = 50.0f;
	public BaseState highHealthState;

	public bool usesInPlayerViewTrigger = false;
	public BaseState inPlayerViewState;

	public bool usesVelocityTrigger = false;
	public float minimumVelocity = 1.0f;
	public string velocityObjectsTag = "Untagged";
	public BaseState velocityExceededState;


	public override string ToString ()
	{
		return name;
	}


#if UNITY_EDITOR
	// === Editor Methods === //

	public abstract void OnStateInspectorEnabled(SerializedObject m_ParentObject);
	protected abstract void DrawStateInspectorEditor(SerializedObject m_Object, AIBehaviors stateMachine);


	public void OnInspectorEnabled(SerializedObject m_ParentObject)
	{
		SerializedObject m_Object = new SerializedObject(this);

		AIBehaviorsAnimationEditorGUI.OnInspectorEnabled(m_ParentObject, m_Object);

		OnStateInspectorEnabled(m_ParentObject);
	}


	public void DrawInspectorEditor(AIBehaviors stateMachine)
	{
		SerializedObject m_Object = new SerializedObject(this);
		bool oldEnabled = GUI.enabled;
		bool drawEnabled = DrawIsEnabled(m_Object);

		GUI.enabled = oldEnabled & drawEnabled;

		AIBehaviorsTriggersGUI.Draw(m_Object, stateMachine);
		EditorGUILayout.Separator();

		AIBehaviorsAnimationEditorGUI.DrawAnimationFields(m_Object);
		EditorGUILayout.Separator();

		DrawMovementOptions(m_Object);
		EditorGUILayout.Separator();

		DrawCooldownProperties(m_Object, stateMachine);
		EditorGUILayout.Separator();

		DrawAudioProperties(m_Object);
		EditorGUILayout.Separator();

		DrawStateInspectorEditor(m_Object, stateMachine);

		m_Object.ApplyModifiedProperties();

		GUI.enabled = oldEnabled;
	}



	public bool DrawIsEnabled(SerializedObject m_Object)
	{
		if ( m_Object.targetObject != null )
		{
			SerializedProperty m_isEnabled = m_Object.FindProperty("isEnabled");
			EditorGUILayout.PropertyField(m_isEnabled);
			m_Object.ApplyModifiedProperties();

			return m_isEnabled.boolValue;
		}

		return false;
	}


	public void DrawPlayerFields(SerializedObject m_ParentObject)
	{
		SerializedProperty m_Prop = m_ParentObject.FindProperty("checkForNewPlayersInterval");
		EditorGUILayout.PropertyField(m_Prop);
	}


	void DrawCooldownProperties(SerializedObject m_State, AIBehaviors fsm)
	{
		SerializedProperty m_Property;

		GUILayout.Label("Cooldown Properties:", EditorStyles.boldLabel);

		m_Property = m_State.FindProperty("cooldownTime");
		EditorGUILayout.PropertyField(m_Property);

		m_Property = m_State.FindProperty("hasCooldownLimit");
		EditorGUILayout.PropertyField(m_Property);

		if ( m_Property.boolValue )
		{
			m_Property = m_State.FindProperty("cooldownLimit");
			EditorGUILayout.PropertyField(m_Property);

			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("Limit Exceeded Transition:");

				m_Property = m_State.FindProperty("cooldownLimitExceededState");
				m_Property.objectReferenceValue = AIBehaviorsStatePopups.DrawEnabledStatePopup(fsm, m_Property.objectReferenceValue as BaseState);
			}
			GUILayout.EndHorizontal();
		}
	}


	public void DrawTransitionStatePopup(AIBehaviors fsm, SerializedObject m_Object, string propertyName)
	{
		DrawTransitionStatePopup("Transition to state:", fsm, m_Object, propertyName);
	}


	public void DrawTransitionStatePopup(string label, AIBehaviors fsm, SerializedObject m_Object, string propertyName)
	{
		SerializedProperty m_InitialState = m_Object.FindProperty(propertyName);
		BaseState state = m_InitialState.objectReferenceValue as BaseState;
		BaseState updatedState;

		EditorGUILayout.Separator();

		GUILayout.Label(label, EditorStyles.boldLabel);
		updatedState = AIBehaviorsStatePopups.DrawEnabledStatePopup(fsm, state);
		if ( updatedState != state )
		{
			m_InitialState.objectReferenceValue = updatedState;
			m_Object.ApplyModifiedProperties();
		}
	}


	public void DrawAudioProperties(SerializedObject m_State)
	{
		SerializedProperty m_Property;

		GUILayout.Label("Audio Properties:", EditorStyles.boldLabel);

		m_Property = m_State.FindProperty("audioClip");
		EditorGUILayout.PropertyField(m_Property);

		m_Property = m_State.FindProperty("audioVolume");
		EditorGUILayout.PropertyField(m_Property);

		m_Property = m_State.FindProperty("loopAudio");
		EditorGUILayout.PropertyField(m_Property);
	}


	public void DrawMovementOptions(SerializedObject m_State)
	{
		SerializedProperty m_property;

		GUILayout.Label("Movement Properties:", EditorStyles.boldLabel);

		// Movement Speed

		m_property = m_State.FindProperty("movementSpeed");
		EditorGUILayout.PropertyField(m_property);

		if ( m_property.floatValue < 0.0f )
			m_property.floatValue = 0.0f;

		// Rotation Speed

		m_property = m_State.FindProperty("rotationSpeed");
		EditorGUILayout.PropertyField(m_property);

		if ( m_property.floatValue < 0.0f )
			m_property.floatValue = 0.0f;
	}
#endif
}