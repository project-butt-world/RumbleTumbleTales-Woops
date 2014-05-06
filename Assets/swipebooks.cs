using UnityEngine;
using System.Collections;

public class swipebooks : MonoBehaviour {

	void OnSwipe( SwipeGesture gesture )
	{
		if( gesture.StartSelection && gesture.StartSelection.rigidbody )
		{
			Vector2 swipeMotion = gesture.Move;
			float speedScale = 2.0f;
			Vector3 force = new Vector3( speedScale * swipeMotion.x, 0, 0 );
			
			// apply the force on the physic object
			gesture.StartSelection.rigidbody.AddForce( force );
		}
	}

	}



