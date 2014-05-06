#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
#else
using UnityEngine;
#endif


public class DefendState : BaseState
{
/*
	public float defendTimePattern = 1.0f;
	public int defendHitPattern = 1;
	public bool useRandomPattern = false;
	public float plusMinusTimePattern = 0.0f;
	public int plusMinusHitPattern = 0;
*/

	public float defensiveTimeLimit = 1.0f;

	public float defensiveBonus = 0.0f;

//	public bool lockPosition = true;
//	public bool lockRotation = true;

	public MonoBehaviour defensiveCallScript = null;
	public string startDefendMethodName = "";
	public string endDefendMethodName = "";

	public float healthAmount = 50.0f;


	protected override void Init(AIBehaviors fsm)
	{
		fsm.isDefending = true;

		if ( defensiveCallScript != null )
		{
			if ( !string.IsNullOrEmpty(startDefendMethodName) )
			{
				defensiveCallScript.SendMessage(startDefendMethodName, this);
			}
		}

		fsm.PlayAudio();
	}


	protected override void StateEnded(AIBehaviors fsm)
	{
		fsm.isDefending = false;

		if ( defensiveCallScript != null )
		{
			if ( !string.IsNullOrEmpty(endDefendMethodName) )
			{
				defensiveCallScript.SendMessage(endDefendMethodName, this);
			}
		}
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
		SerializedProperty m_Property = null;

		EditorGUILayout.Separator();
		GUILayout.Label("Defensive Properties:", EditorStyles.boldLabel);

		m_Property = m_State.FindProperty("defensiveBonus");
		EditorGUILayout.PropertyField(m_Property);

//		m_Property = m_State.FindProperty("lockPosition");
//		EditorGUILayout.PropertyField(m_Property);

//		m_Property = m_State.FindProperty("lockRotation");
//		EditorGUILayout.PropertyField(m_Property);

		GetScriptsToCall(m_State, stateMachine);

		m_State.ApplyModifiedProperties();
	}


	protected void GetScriptsToCall(SerializedObject m_State, AIBehaviors fsm)
	{
		Component[] components = AIBehaviorsComponentInfoHelper.GetNonFSMComponents(fsm.gameObject);
		List<string> includedComponents = new List<string>();
		Dictionary<Component, List<string>> methodsPerComponent = new Dictionary<Component, List<string>>();
		int selectedComponent = 0;
		string selectedComponentType = "";
		SerializedProperty m_Property;

		if ( defensiveCallScript != null )
		{
			selectedComponentType = defensiveCallScript.GetType().ToString();
		}

		foreach ( Component component in components )
		{
			MethodInfo[] methods = component.GetType().GetMethods();

			foreach ( MethodInfo method in methods )
			{
				ParameterInfo[] parameters = method.GetParameters();

				if ( parameters.Length == 1 )
				{
					if ( parameters[0].ParameterType == this.GetType() )
					{
						if ( !methodsPerComponent.ContainsKey(component) )
						{
							string componentType = component.GetType().ToString();

							includedComponents.Add(componentType);
							methodsPerComponent[component] = new List<string>();

							if ( selectedComponentType == componentType )
								selectedComponent = includedComponents.Count - 1;
						}

						methodsPerComponent[component].Add(method.Name);
					}
				}
			}
		}

		if ( includedComponents.Count > 0 )
		{
			Component component = null;
			int newIndex = 0;

			EditorGUILayout.Separator();
			GUILayout.Label("Callback Properties:", EditorStyles.boldLabel);

			newIndex = EditorGUILayout.Popup("Defend callback script:", selectedComponent, includedComponents.ToArray());

			m_Property = m_State.FindProperty("defensiveCallScript");
			//EditorGUILayout.PropertyField(m_Property);
			component = fsm.gameObject.GetComponent(includedComponents[selectedComponent]);

			if ( newIndex != selectedComponent || m_Property.objectReferenceValue == null )
			{
				m_Property.objectReferenceValue = component as MonoBehaviour;
			}

			if ( m_Property.objectReferenceValue != null )
			{
				MethodInfo[] methods = m_Property.objectReferenceValue.GetType().GetMethods();

				if ( methods.Length > 0 )
				{
					List<string> availableMethodsList = methodsPerComponent[component];
					string[] availableMethods = availableMethodsList.ToArray();
					int startIndex = 0;
					int endIndex = 0;

					// === Draw the start state method === //

					m_Property = m_State.FindProperty("startDefendMethodName");
					startIndex = availableMethodsList.IndexOf(m_Property.stringValue);

					if ( startIndex < 0 )
						startIndex = 0;

					newIndex = EditorGUILayout.Popup("Start Defend callback:", startIndex, availableMethods);
					//EditorGUILayout.PropertyField(m_Property);

					if ( newIndex != startIndex || m_Property.stringValue != availableMethods[newIndex] )
						m_Property.stringValue = availableMethods[newIndex];

					// === Draw the end state method === //

					m_Property = m_State.FindProperty("endDefendMethodName");
					endIndex = availableMethodsList.IndexOf(m_Property.stringValue);

					if ( endIndex < 0 )
						endIndex = 0;

					newIndex = EditorGUILayout.Popup("Stop Defend callback:", endIndex, availableMethods);
					//EditorGUILayout.PropertyField(m_Property);

					if ( newIndex != endIndex || m_Property.stringValue != availableMethods[newIndex] )
						m_Property.stringValue = availableMethods[newIndex];
				}
			}
		}
		else
		{
			AIBehaviorsCodeSampleGUI.Draw(typeof(DefendState), "defendState", 2, new string[2] { "OnStartDefending", "OnStopDefending" } );
		}
	}
#endif
}