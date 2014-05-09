using UnityEngine;
using System.Collections;

public class gogomagic : MonoBehaviour {
	
	void Start(){
		
		changer ();
	}
	
	void changer(){
		AutoFade.LoadLevel (6, 3, 1, Color.black);
	}
}
