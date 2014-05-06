using UnityEngine;


public class PlayerStats : MonoBehaviour
{
	public float health = 100.0f;


	public void SubtractHealth(float amount)
	{
		health -= amount;

		if ( health <= 0.0f )
		{
			health = 0.0f;
			Debug.LogWarning("You're Dead!");
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if ( other.GetComponent<ProjectileCollider>() != null )
		{
			SubtractHealth(10);
		}
	}
}