using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


[CustomEditor(typeof(AIBehaviorsAnimationStates))]
public class AIBehaviorsAnimationStatesEditor : Editor
{
	SerializedObject m_Object;
	SerializedProperty animationStatesProp;
	SerializedProperty m_AnimationStatesCount;

	const string kArraySize = "states.Array.size";
	const string kArrayData = "states.Array.data[{0}]";

	Transform transform;
	AIBehaviorsAnimationStates animStates;
	GameObject statesGameObject;

	AIBehaviorsStyles styles = new AIBehaviorsStyles();


	void OnEnable()
	{
		m_Object = new SerializedObject(target);
		animationStatesProp = m_Object.FindProperty("states");
		m_AnimationStatesCount = m_Object.FindProperty(kArraySize);

		animStates = m_Object.targetObject as AIBehaviorsAnimationStates;
		transform = animStates.transform;

		InitStatesGameObject();
	}


	void InitStatesGameObject()
	{
		SerializedProperty m_Prop = m_Object.FindProperty("animationStatesGameObject");

		statesGameObject = m_Prop.objectReferenceValue as GameObject;

		if ( statesGameObject == null )
		{
			statesGameObject = new GameObject("AnimationStates");
			m_Prop.objectReferenceValue = statesGameObject;

			statesGameObject.transform.parent = transform;
			statesGameObject.transform.localPosition = Vector3.zero;

			m_Object.ApplyModifiedProperties();
		}
	}


	public override void OnInspectorGUI()
	{
		m_Object.Update();

		int arraySize = m_AnimationStatesCount.intValue;
		AIBehaviorsAnimationState[] states = new AIBehaviorsAnimationState[arraySize+1];
		SerializedProperty is3D = m_Object.FindProperty("is3D");

		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(is3D);

		for ( int i = 0; i < arraySize; i++ )
		{
			string stateNameLabel = "";
			bool oldEnabled = GUI.enabled;

			if ( m_Object.FindProperty(string.Format(kArrayData, i)) == null )
			{
				AIBehaviorsAssignableObjectArray.RemoveObjectAtIndex(m_Object, i, "states");
				continue;
			}

			states[i] = m_Object.FindProperty(string.Format(kArrayData, i)).objectReferenceValue as AIBehaviorsAnimationState;

			if ( states[i] == null )
			{
				AIBehaviorsAssignableObjectArray.RemoveObjectAtIndex(m_Object, i, "states");
				continue;
			}

			GUILayout.BeginHorizontal();

			if ( states[i].name == null || states[i].name == "" )
				stateNameLabel = "Untitled animation";
			else
				stateNameLabel = states[i].name;

			states[i].foldoutOpen = EditorGUILayout.Foldout(states[i].foldoutOpen, stateNameLabel, EditorStyles.foldoutPreDrop);

			GUI.enabled = i > 0;
			if ( GUILayout.Button(styles.blankContent, styles.upStyle, GUILayout.MaxWidth(styles.arrowButtonWidths)) )
			{
				animationStatesProp.MoveArrayElement(i, i-1);
			}

			GUI.enabled = i < arraySize-1;
			if ( GUILayout.Button(styles.blankContent, styles.downStyle, GUILayout.MaxWidth(styles.arrowButtonWidths)) )
			{
				animationStatesProp.MoveArrayElement(i, i+1);
			}

			GUI.enabled = true;
			if ( GUILayout.Button(styles.blankContent, styles.addStyle, GUILayout.MaxWidth(styles.addRemoveButtonWidths)) )
			{
				animationStatesProp.InsertArrayElementAtIndex(i);
				animationStatesProp.GetArrayElementAtIndex(i+1).objectReferenceValue = statesGameObject.AddComponent<AIBehaviorsAnimationState>();
			}

			GUI.enabled = arraySize > 1;
			if ( GUILayout.Button(styles.blankContent, styles.removeStyle, GUILayout.MaxWidth(styles.addRemoveButtonWidths)) )
			{
				AIBehaviorsAssignableObjectArray.RemoveObjectAtIndex(m_Object, i, "states");
				DestroyImmediate(m_Object.targetObject as AIBehaviorsAnimationState);
				break;
			}
			GUI.enabled = oldEnabled;

			GUILayout.Space(10);

			GUILayout.EndHorizontal();

			GUILayout.Space(2);

			if ( states[i].foldoutOpen )
				DrawAnimProperties(states[i], is3D.boolValue);
		}

		if ( arraySize == 0 )
		{
			m_Object.FindProperty(kArraySize).intValue++;
			animationStatesProp.GetArrayElementAtIndex(0).objectReferenceValue = statesGameObject.AddComponent<AIBehaviorsAnimationState>();
		}

		EditorGUILayout.Separator();

		m_Object.ApplyModifiedProperties();
	}


	private void InsertArrayElement(SerializedProperty animationStatesProp, int index)
	{
	}


	public static void DrawAnimProperties(AIBehaviorsAnimationState animState, bool is3D)
	{
		SerializedObject m_animState = new SerializedObject(animState);
		string speedTooltip = "";

		m_animState.Update();

		SerializedProperty m_animName = m_animState.FindProperty("name");
		SerializedProperty m_speed = m_animState.FindProperty("speed");
		SerializedProperty m_wrapMode = m_animState.FindProperty("animationWrapMode");

		EditorGUILayout.Separator();

		EditorGUILayout.PropertyField(m_animName);

		if ( !is3D )
		{
			SerializedProperty m_startFrame = m_animState.FindProperty("startFrame");
			SerializedProperty m_endFrame = m_animState.FindProperty("endFrame");
			EditorGUILayout.PropertyField(m_startFrame);
			EditorGUILayout.PropertyField(m_endFrame);

			if ( m_startFrame.intValue < 0 )
				m_startFrame.intValue = 0;

			if ( m_endFrame.intValue < 0 )
				m_endFrame.intValue = 0;

			speedTooltip = "This is a frames persecond value, IE: 30";
		}
		else
		{
			speedTooltip = "This is a normalized value\n\n0.5 = half speed\n1.0 = normal speed\n2.0 = double speed";
		}


		EditorGUILayout.PropertyField(m_speed, new GUIContent("Speed " + (is3D ? "" : "(FPS)"), speedTooltip));
		EditorGUILayout.PropertyField(m_wrapMode);

		EditorGUILayout.Separator();

		m_animState.ApplyModifiedProperties();
	}
}