using UnityEngine;


public class ExampleGotHitComponent : MonoBehaviour
{
	public float projectileDamage = 10.0f;
	AIBehaviors fsm = null;


	void Awake()
	{
		fsm = GetComponent<AIBehaviors>();
	}


	// Update is called once per frame
	void OnTriggerEnter (Collider col)
	{
		if ( col.GetComponent<ProjectileCollider>() != null )
		{
			fsm.GotHit(projectileDamage);
		}
	}


	public void OnStartDefending(DefendState defendState)
	{
		// Code here for when the defend state begins
		//    Use defendState.defensiveBonus to get the defensive bonus value
	}


	public void OnStopDefending(DefendState defendState)
	{
		// Code here for when the defend state ends
	}
}