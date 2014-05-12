using UnityEngine;
using System.Collections;

public class inventorylerp : MonoBehaviour {


	public Transform target;
	public float smoothTime = 0.1F;
	private Vector3 velocity = Vector3.zero;


//	public Vector3 Invdown;
//	public Vector3 InvUp;

	void Start () {
	
//		Invdown = this.transform.position;
//		InvUp = new Vector3(Invdown.x, -2, 39);
	}
	

	void Update () {

	//	Invdown.position.y

	}

	void OnMouseDown(){

	//	Debug.Log ("inv");
	//	transform.position = Vector3.Lerp(Invdown,InvUp ,Time.deltaTime);
		Vector3 targetPosition = target.TransformPoint(new Vector3(0, 5, -10));
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

		
	}
}





//	public Transform startMarker;
//	public Transform endMarker;
//	public float speed = 1.0F;
//	private float startTime;
//	private float journeyLength;
//	public Transform target;
//	public float smooth = 5.0F;
//	void Start() {
//		startTime = Time.time;
//		journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
//	}
//	void Update() {
//		float distCovered = (Time.time - startTime) * speed;
//		float fracJourney = distCovered / journeyLength;
//		transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
//	}
//}

 
