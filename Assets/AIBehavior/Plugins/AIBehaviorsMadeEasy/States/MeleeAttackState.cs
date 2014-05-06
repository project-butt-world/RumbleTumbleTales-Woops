#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#else
using UnityEngine;
#endif


public class MeleeAttackState : AttackState
{
	void Awake()
	{
		if ( enabled )
		{
			Debug.LogWarning("Please click on '" + transform.parent.name + "' in order to upgrade it as it uses the legacy state 'MeleeAttackState' class.");
		}
	}


#if UNITY_EDITOR
	// === Editor Functions === //

	public override void OnStateInspectorEnabled(SerializedObject m_ParentObject)
	{
		UpgradeLegacyDerivedState(m_ParentObject);
	}
#endif
}