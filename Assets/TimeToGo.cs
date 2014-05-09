using UnityEngine;
using System.Collections;

public class TimeToGo : MonoBehaviour {

	
	void Start(){
		
		changee ();
	}
	
	void changee(){
		AutoFade.LoadLevel (6, 3, 1, Color.black);
	}
}

