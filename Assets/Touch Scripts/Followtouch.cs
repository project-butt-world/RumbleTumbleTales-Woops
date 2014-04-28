/*/
* Script by Devin Curry
	* www.Devination.com
		* www.youtube.com/user/curryboy001
		* Please like and subscribe if you found my tutorials helpful :D
			* Google+ Community: https://plus.google.com/communities/108584850180626452949
				* Twitter: https://twitter.com/Devination3D
				* Facebook Page: https://www.facebook.com/unity3Dtutorialsbydevin
				/*/
				using UnityEngine;
using System.Collections;

public class FollowTouch : TouchLogicV2 
{
	public float speed = 1f;
	private Vector3 finger;
	private Transform myTrans, camTrans;
	
	void Start () 
	{
		myTrans = this.transform;
		camTrans = Camera.main.transform;
	}
	
	//separated to own function so it can be called from multiple functions
	void LookAtFinger()
	{
		//z of ScreenToWorldPoint is distance from camera into the world, so we need to find this object's distance from the camera to make it stay on the same plane
		Vector3 tempTouch = new Vector3(Input.GetTouch(touch2Watch).position.x, Input.GetTouch(touch2Watch).position.y,camTrans.position.y - myTrans.position.y);
		//Convert screen to world space
		finger = Camera.main.ScreenToWorldPoint(tempTouch);
		
		//look at finger position
		myTrans.LookAt(finger);
		
		//move towards finger if not too close
//		if(Vector3.Distance(finger, myTrans.position) > maxDist)
//			myTrans.Translate (Vector3.forward * speed * Time.deltaTime);
	}
	void OnTouchMovedAnywhere()
	{
		LookAtFinger();
	}
	void OnTouchStayedAnywhere()
	{
		LookAtFinger();
	}
	void OnTouchBeganAnywhere()
	{
		touch2Watch = TouchLogic.currTouch;
	}
}