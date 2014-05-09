using UnityEngine;
using System.Collections;

public class TBFollowFinger : MonoBehaviour
{
	public int FingerIndex = 0;


	//add in smoothing
	public Transform startMarker;
	public Transform endMarker;
	public float speed = 1.0F;
	private float startTime;
	private float journeyLength;
	public Transform target;
	public float smooth = 5.0F;

	void Start() {
		startTime = Time.time;
		journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
	}

	
	void Update()
	{
		//smoothing update

		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
	//	transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);


		//end smoothing update




		// look up the desired finger
		FingerGestures.Finger finger = FingerGestures.GetFinger( FingerIndex );
		
		// finger must be down
		if( !finger || !finger.IsDown )
			return;
		
		// get world-space position from finger's screen-space position
		Vector3 p = Camera.main.ScreenToWorldPoint( finger.Position );
	
//		transform.Translate (-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
	//	p.z =  Vector2.Lerp (transform.position, Player.position, cameraSpeed * Time.fixedDeltaTime); 
			//transform.position.z*speed;
		
		// finally, update our object's position
		transform.position = p;
	}
}








