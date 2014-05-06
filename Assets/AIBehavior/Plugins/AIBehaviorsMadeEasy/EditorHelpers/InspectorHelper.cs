#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections;


public class InspectorHelper
{
	public static void DrawInspector(SerializedObject sObject)
	{
		Type type = sObject.targetObject.GetType();
		BindingFlags bindingFlags = AIBehaviorsComponentInfoHelper.standardBindingFlags;
		FieldInfo[] fields = type.GetFields(bindingFlags);
		string baseStateTypeString = (typeof(BaseState)).ToString();
		SerializedProperty prop;

		foreach ( FieldInfo field in fields )
		{
			if ( field.DeclaringType == type )
			{
				prop = sObject.FindProperty(field.Name);

				if ( prop == null )
					continue;

				if ( prop.isArray && prop.propertyType != SerializedPropertyType.String )
				{
					DrawArray(sObject, field.Name);
				}
				else
				{
					bool isBaseState = prop.type.Contains(baseStateTypeString);

					if ( isBaseState )
					{
						GameObject targetObject = (sObject.targetObject as Component).gameObject;
						AIBehaviors fsm = targetObject.transform.parent.GetComponent<AIBehaviors>();

						prop.objectReferenceValue = AIBehaviorsStatePopups.DrawEnabledStatePopup(fsm, prop.objectReferenceValue as BaseState) as BaseState;
					}
					else
					{
						EditorGUILayout.PropertyField(prop);
					}
				}
			}
		}
	}


	private static void DrawArray(SerializedObject sObject, string fieldName)
	{
		SerializedProperty arraySizeProperty = sObject.FindProperty(fieldName + ".Array.size");
		SerializedProperty arrayDataProperty;
		SerializedProperty prop;
		string arrayDataPropertyName = fieldName + ".Array.data[{0}]";
		string baseStateTypeString = (typeof(BaseState)).ToString();
		AIBehaviorsStyles styles = new AIBehaviorsStyles();

		prop = sObject.FindProperty(fieldName);

		GUILayout.BeginHorizontal();
		{
			GUILayout.Label(fieldName.ToUpper() + ": ");
			EditorGUILayout.PropertyField(arraySizeProperty);
		}
		GUILayout.EndHorizontal();

		for ( int i = 0; i < prop.arraySize; i++ )
		{
			bool isBaseState;
			bool oldEnabled = GUI.enabled;

			GUILayout.BeginHorizontal();
			{
				arrayDataProperty = sObject.FindProperty(string.Format(arrayDataPropertyName, i));
				isBaseState = arrayDataProperty.type.Contains(baseStateTypeString);

				if ( isBaseState )
				{
					GameObject targetObject = (sObject.targetObject as Component).gameObject;
					AIBehaviors fsm = targetObject.transform.parent.GetComponent<AIBehaviors>();
					BaseState curState = arrayDataProperty.objectReferenceValue as BaseState;

					arrayDataProperty.objectReferenceValue = AIBehaviorsStatePopups.DrawEnabledStatePopup(fsm, curState) as BaseState;
				}
				else
				{
					EditorGUILayout.PropertyField(arrayDataProperty);
				}

				GUI.enabled = i > 0;
				if ( GUILayout.Button(styles.blankContent, styles.upStyle, GUILayout.MaxWidth(styles.arrowButtonWidths)) )
				{
					prop.MoveArrayElement(i, i-1);
				}
				GUI.enabled = oldEnabled;

				GUI.enabled = i < prop.arraySize - 1;
				if ( GUILayout.Button(styles.blankContent, styles.downStyle, GUILayout.MaxWidth(styles.arrowButtonWidths)) )
				{
					prop.MoveArrayElement(i, i+1);
				}
				GUI.enabled = oldEnabled;

				if ( GUILayout.Button(styles.blankContent, styles.addStyle, GUILayout.MaxWidth(styles.addRemoveButtonWidths)) )
				{
					prop.InsertArrayElementAtIndex(i);
				}
				GUI.enabled = oldEnabled;

				if ( GUILayout.Button(styles.blankContent, styles.removeStyle, GUILayout.MaxWidth(styles.addRemoveButtonWidths)) )
				{
					prop.DeleteArrayElementAtIndex(i);
				}
				GUI.enabled = oldEnabled;
			}
			GUILayout.EndHorizontal();
		}
	}
}
#endif