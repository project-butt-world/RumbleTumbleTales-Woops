using UnityEngine;
using System.Collections;

public class gofish : MonoBehaviour {

	
	public Animator fizzanim;
	public bool tapped;
//	GameObject fizzpoz;
	
	void Start() {
	//	fizzpoz = GameObject.Find("Princess Fizz");
	//	fizzanim = GetComponent (fizzpoz.Animator);
	
		tapped = false;
		
	}
	
	void Update(){
		
		//	if (tapped = true) {
		//	Debug.Log("tapped");
		//	dooranim.Play ("DoorOpenYellow");
	}
	
	//	void OnFingerDown(FingerDownEvent e)
	void Onswamp( TapGesture gesture ) 
	{
		if( gesture.Selection)
			Debug.Log( "Tapped object: " + gesture.Selection.name );
		//  dooranim.Play ("DoorOpenYellow");
		fizzanim.CrossFade("Fizz_Fishing",0.6f);

		//Object.Destroy(this, 0.9f);
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
