#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#else
using UnityEngine;
#endif


public class SeekAmmoState : SeekState
{
	void Awake()
	{
		if ( enabled )
		{
			Debug.LogWarning("Please click on '" + transform.parent.name + "' in order to upgrade it as it uses the legacy state 'SeekAmmoState' class.");
		}
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