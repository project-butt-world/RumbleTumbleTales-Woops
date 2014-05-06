using UnityEngine;


public class ExampleProjectile : MonoBehaviour
{
	public float damage = 0.0f;

	public GameObject explosionPrefab;


	void Awake()
	{
		rigidbody.AddRelativeForce(Vector3.forward * 1000.0f);
		Destroy(gameObject, 10.0f);
	}


	void OnTriggerEnter(Collider col)
	{
		if ( col.gameObject.tag == "Player" )
		{
			PlayerStats playerStats = col.GetComponent<PlayerStats>();

			if ( playerStats != null )
			{
				playerStats.SubtractHealth(damage);
				Debug.LogWarning("Attack: " + damage + " : health=" + playerStats.health );
			}

			SpawnFlames();
		}
	}


	void OnCollisionEnter(Collision col)
	{
		SpawnFlames();
	}


	void SpawnFlames()
	{
		Destroy(gameObject);
		Instantiate(explosionPrefab, transform.position, Quaternion.identity);
	}
}