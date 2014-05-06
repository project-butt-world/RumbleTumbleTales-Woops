#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
#else
using UnityEngine;
#endif


public class AttackState : BaseState
{
	public float minAttackDistance = 0.0f;
	public float maxAttackDistance = 1.0f;

	public float attackSpeed = 1.0f;
	public float attackDamage = 10.0f;

	public float reloadTime = 1.0f;
	public bool hasReloadLimit = false;
	public int reloadLimit = 0;

	public float healthAmount = 0.0f;

	public string attackAnimName = "";
	public string reloadAnimName = "";
	public float attackPoint = 0.5f;
	public float animAttackTime = 0.0f;

	public BaseState doneAttackingState;

	public Component scriptWithAttackMethod;
	public string methodName = "";

	private Animation attackAnimation;
	private float previousSamplePosition = 0.0f;


	protected override void Init(AIBehaviors fsm)
	{
		fsm.MoveAgent(fsm.transform, 0.0f, rotationSpeed);
		attackAnimation = fsm.gameObject.GetComponentInChildren<Animation>();
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
		Transform target = fsm.GetClosestPlayer();

		if ( target != null )
		{
			fsm.RotateAgent(target, rotationSpeed);
		}

		AIBehaviorsAnimationState animState = fsm.animationStates.GetStateWithName(attackAnimName);
		fsm.PlayAnimation(animState);

		if ( scriptWithAttackMethod != null )
		{
			if ( !string.IsNullOrEmpty(methodName) )
			{
				if ( attackAnimation != null && attackAnimation[attackAnimName] != null )
				{
					float curAnimPosition = attackAnimation[attackAnimName].normalizedTime % 1.0f;

					if ( previousSamplePosition > attackPoint || curAnimPosition < attackPoint )
					{
						previousSamplePosition = curAnimPosition;
						return;
					}

					previousSamplePosition = curAnimPosition;
				}

				scriptWithAttackMethod.SendMessage(methodName, new AIBehaviors_AttackData(attackDamage));
				fsm.PlayAudio();
			}
		}
	}


#if UNITY_EDITOR
	// === Editor Methods === //

	public override void OnStateInspectorEnabled(SerializedObject m_ParentObject)
	{
	}


	protected override void DrawStateInspectorEditor(SerializedObject m_State, AIBehaviors stateMachine)
	{
		SerializedProperty m_property;

		string[] animNames = AIBehaviorsAnimationEditorGUI.GetAnimationStateNames(m_State);
		int curAttackAnimIndex = -1, newAttackAnimIndex = 0;
		int curReloadAnimIndex = -1, newReloadAnimIndex = 0;

		for ( int i = 0; i < animNames.Length; i++ )
		{
			if ( animNames[i] == attackAnimName )
			{
				curAttackAnimIndex = i;
			}

			if ( animNames[i] == reloadAnimName )
			{
				curReloadAnimIndex = i;
			}
		}

		GUILayout.Label("Attack Distances:", EditorStyles.boldLabel);

		float minDistance = 0.0f;
		m_property = m_State.FindProperty("minAttackDistance");
		EditorGUILayout.PropertyField(m_property, new GUIContent("Min Distance"));
		if ( m_property.floatValue < 0.0f )
			m_property.floatValue = 0.0f;
		minDistance = m_property.floatValue;

		m_property = m_State.FindProperty("maxAttackDistance");
		EditorGUILayout.PropertyField(m_property, new GUIContent("Max Distance"));
		if ( m_property.floatValue < minDistance )
			m_property.floatValue = minDistance;

		GUILayout.Label("Attack Values:", EditorStyles.boldLabel);

		m_property = m_State.FindProperty("attackSpeed");
		EditorGUILayout.PropertyField(m_property);

		m_property = m_State.FindProperty("attackDamage");
		EditorGUILayout.PropertyField(m_property);

		// === Reloading === //

		GUILayout.Label("Reloading Values:", EditorStyles.boldLabel);

		m_property = m_State.FindProperty("reloadTime");
		EditorGUILayout.PropertyField(m_property);
		if ( m_property.floatValue < 0.0f )
			m_property.floatValue = 0.0f;

		m_property = m_State.FindProperty("hasReloadLimit");
		EditorGUILayout.PropertyField(m_property);

		if ( m_property.boolValue )
		{
			m_property = m_State.FindProperty("reloadLimit");
			EditorGUILayout.PropertyField(m_property);
		}

		// Animation Settings

		GUILayout.Label("Animation Settings:", EditorStyles.boldLabel);

		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("Reload animation:");
			newReloadAnimIndex = EditorGUILayout.Popup(curReloadAnimIndex, animNames);

			if ( newReloadAnimIndex != curReloadAnimIndex )
			{
				m_State.FindProperty("reloadAnimName").stringValue = animNames[newReloadAnimIndex];
				curReloadAnimIndex = newReloadAnimIndex;
			}
		}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("Attack animation:");
			newAttackAnimIndex = EditorGUILayout.Popup(curAttackAnimIndex, animNames);

			if ( newAttackAnimIndex != curAttackAnimIndex )
			{
				m_State.FindProperty("attackAnimName").stringValue = animNames[newAttackAnimIndex];
				curAttackAnimIndex = newAttackAnimIndex;
			}
		}
		GUILayout.EndHorizontal();

		m_property = m_State.FindProperty("attackPoint");
		EditorGUILayout.Slider(m_property, 0.0f, 1.0f);

		if ( !Application.isPlaying )
		{
			if ( curAttackAnimIndex != -1 && curAttackAnimIndex < animNames.Length )
			{
				float calcAttackTime = SampleAttackAnimation(stateMachine, animNames[newAttackAnimIndex], m_property.floatValue);

				m_property = m_State.FindProperty("animAttackTime");
				m_property.floatValue = calcAttackTime;
			}
		}

		// === Attack Method === //

		GUILayout.Label("Attack Method:", EditorStyles.boldLabel);

		Component[] components = GetAttackMethodComponents(stateMachine.gameObject);
		int selectedComponent = -1, newSelectedComponent = 0;

		if ( components.Length > 0 )
		{
			string[] componentNames = GetAttackMethodComponentNames(components);

			for ( int i = 0; i < components.Length; i++ )
			{
				if ( components[i] == scriptWithAttackMethod )
				{
					selectedComponent = i;
					break;
				}
			}

			newSelectedComponent = EditorGUILayout.Popup(selectedComponent, componentNames);

			if ( selectedComponent != newSelectedComponent )
			{
				m_property = m_State.FindProperty("scriptWithAttackMethod");
				m_property.objectReferenceValue = components[newSelectedComponent];
			}
		}
		else
		{
			AIBehaviorsCodeSampleGUI.Draw(typeof(AIBehaviors_AttackData), "attackData", "OnAttack");
		}

		if ( components.Length > 0 )
		{
			string[] methodNames = GetAttackMethodNamesForComponent(components[selectedComponent < 0 ? 0 : selectedComponent]);
			int curSelectedMethod = -1, newSelectedMethod = 0;

			for ( int i = 0; i < methodNames.Length; i++ )
			{
				if ( methodNames[i] == methodName )
				{
					curSelectedMethod = i;
					break;
				}
			}

			newSelectedMethod = EditorGUILayout.Popup(curSelectedMethod, methodNames);
	
			if ( curSelectedMethod != newSelectedMethod )
			{
				m_property = m_State.FindProperty("methodName");
				m_property.stringValue = methodNames[newSelectedMethod];
			}
		}

		m_State.ApplyModifiedProperties();
	}


	Component[] GetAttackMethodComponents(GameObject fsmGO)
	{
		Component[] components = AIBehaviorsComponentInfoHelper.GetNonFSMComponents(fsmGO);
		List<Component> componentList = new List<Component>();

		foreach ( Component component in components )
		{
			if ( GetAttackMethodNamesForComponent(component).Length > 0 )
				componentList.Add(component);
		}

		return componentList.ToArray();
	}


	string[] GetAttackMethodComponentNames(Component[] components)
	{
		string[] componentNames = new string[components.Length];

		for ( int i = 0; i < components.Length; i++ )
		{
			componentNames[i] = components[i].GetType().ToString();
		}

		return componentNames;
	}


	string[] GetAttackMethodNamesForComponent(Component component)
	{
		if ( component != null )
		{
			List<string> methodNames = new List<string>();
			Type type = component.GetType();
			MethodInfo[] methods = type.GetMethods();

			foreach ( MethodInfo mi in methods )
			{
				ParameterInfo[] parameters = mi.GetParameters();

				if ( parameters.Length == 1 )
				{
					if ( parameters[0].ParameterType == typeof(AIBehaviors_AttackData) )
					{
						methodNames.Add(mi.Name);
					}
				}
			}

			return methodNames.ToArray();
		}

		return new string[0];
	}


	float SampleAttackAnimation(AIBehaviors stateMachine, string clipName, float position)
	{
		Animation anim = stateMachine.gameObject.GetComponentInChildren<Animation>();

		if ( anim != null )
		{
			AnimationClip clip = anim.GetClip(clipName);

			if ( clip != null )
			{
				anim.Play(clip.name);
				anim[clip.name].normalizedTime = position;
				anim.Sample();
				anim[clip.name].normalizedTime = 0.0f;

				return anim[clip.name].length * position;
			}
		}

		return 0.0f;
	}


	// TODO: Deprecate this
	protected void UpgradeLegacyDerivedState(SerializedObject m_ParentObject)
	{
		AIBehaviors ai = m_ParentObject.targetObject as AIBehaviors;
		BaseState[] states = ai.GetAllStates();
		Transform selectedTransform = Selection.activeTransform;
		AttackState attackState = null;

		Undo.RegisterSceneUndo("Replaced as AttackState");

		if ( selectedTransform == null )
			selectedTransform = ai.transform;

		foreach ( Transform tfm in selectedTransform )
		{
			if ( tfm.name == "States" && tfm.parent == selectedTransform )
			{
				attackState = tfm.gameObject.AddComponent<AttackState>();
			}
		}

		attackState.name = name;

		attackState.minAttackDistance = minAttackDistance;
		attackState.maxAttackDistance = maxAttackDistance;

		attackState.attackSpeed = attackSpeed;
		attackState.attackDamage = attackDamage;

		attackState.reloadTime = reloadTime;
		attackState.hasReloadLimit = hasReloadLimit;
		attackState.reloadLimit = reloadLimit;

		attackState.healthAmount = healthAmount;

		attackState.attackAnimName = attackAnimName;
		attackState.reloadAnimName = reloadAnimName;
		attackState.attackPoint = attackPoint;
		attackState.animAttackTime = animAttackTime;

		attackState.doneAttackingState = doneAttackingState;

		attackState.scriptWithAttackMethod = scriptWithAttackMethod;
		attackState.methodName = methodName;

		attackState.triggers = triggers;

		for ( int i = 0; i < states.Length; i++ )
		{
			const string kArrayData = "states.Array.data[{0}]";
			SerializedProperty prop = m_ParentObject.FindProperty(string.Format(kArrayData, i));

			if ( states[i] == this )
			{
				prop.objectReferenceValue = attackState;
			}
		}

		foreach ( BaseState state in states )
		{
			foreach ( BaseTrigger trigger in state.triggers )
			{
				if ( trigger.transitionState == this )
				{
					SerializedObject sObject = new SerializedObject(trigger);
					SerializedProperty prop = sObject.FindProperty("transitionState");
					prop.objectReferenceValue = attackState;
					sObject.ApplyModifiedProperties();
				}
			}
		}

		m_ParentObject.ApplyModifiedProperties();

		DestroyImmediate(this, true);
	}
#endif
}


public class AIBehaviors_AttackData
{
	public float damage;

	public AIBehaviors_AttackData(float damage)
	{
		this.damage = damage;
	}
}