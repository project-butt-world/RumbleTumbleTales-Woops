using UnityEngine;


public class AIBehaviorsCharacterAnimator : MonoBehaviour
{
	Animation anim = null;
	bool hasAnimationComponent = false;


	void Awake()
	{
		anim = GetComponentInChildren<Animation>();

		hasAnimationComponent = anim != null;

		if ( !hasAnimationComponent )
		{
			Debug.LogWarning("No animation component found for the '" + gameObject.name + "' object or child objects");
		}
	}


	public void OnAnimationState(AIBehaviorsAnimationState animState)
	{
		if ( hasAnimationComponent && animState != null )
		{
			string stateName = animState.name;

			if ( anim[stateName] != null )
			{
				anim[stateName].wrapMode = animState.animationWrapMode;
				anim[stateName].speed = animState.speed;
				anim.CrossFade(stateName);
			}
			else
				Debug.LogWarning("The animation state \"" + stateName + "\" couldn't be found.");
		}
	}
}