using UnityEngine;
using System.Collections;

public class DoStuffOnTap : MonoBehaviour {

	public Animator dooranim;
	public bool tapped;

	void Start() {
		dooranim = GetComponent<Animator>();
		tapped = false;

	}

	void Update(){

	//	if (tapped = true) {
		//	Debug.Log("tapped");
					//	dooranim.Play ("DoorOpenYellow");
				}
		
//	void OnFingerDown(FingerDownEvent e)
	void OnTap( TapGesture gesture ) 
	{
		if( gesture.Selection)
			Debug.Log( "Tapped object: " + gesture.Selection.name );
		  //  dooranim.Play ("DoorOpenYellow");
		      dooranim.CrossFade("DoorOpenYellow", 0.9F);
		Object.Destroy(this, 0.9f);
		     // tapped = true;
		   // this.transform.Translate(Vector2.up * Time.deltaTime, Space.World);
		//    dooranim.CrossFade("DoorOpenYellow", 1.0);
				    
	//	WaitForSeconds (3);

		//	    dooranim.SetBool("open",true);
//		else
//			Debug.Log( "No object was tapped at " + gesture.Position );
//	}
	//	Debug.Log( "Tap gesture detected at " + gesture.Position + 
//		           ". It was sent by " + gesture.Recognizer.name );

}


}