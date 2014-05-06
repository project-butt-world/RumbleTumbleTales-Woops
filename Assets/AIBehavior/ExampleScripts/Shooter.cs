using UnityEngine;


public class Shooter : MonoBehaviour
{
	Camera mainCamera = null;


	void Start()
	{
		mainCamera = Camera.main;
	}


	// Update is called once per frame
	void Update ()
	{
		if ( Input.GetMouseButtonDown(0) )
		{
			Ray mouseRay = mainCamera.ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
			GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			Transform tfm = go.transform;
			Rigidbody rb = go.AddComponent<Rigidbody>();

			go.renderer.useLightProbes = true;
			go.AddComponent<ProjectileCollider>();

			tfm.position = mouseRay.origin + mouseRay.direction * 0.5f;
			tfm.localScale *= 0.25f;

			rb.AddForce(mouseRay.direction * 1500.0f);

			go.tag = "Respawn";
		}
	}
}