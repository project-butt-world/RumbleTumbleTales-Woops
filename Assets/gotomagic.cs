using UnityEngine;
using System.Collections;

public class gotomagic : MonoBehaviour {

	void Onlvl1( TapGesture gesture ) 
	{
		if( gesture.Selection )
			Debug.Log( "Tapped object: " + gesture.Selection.name );
		AutoFade.LoadLevel(4 ,3,1,Color.black);
		//	    Application.LoadLevel (1);
		
		//		else
		//			Debug.Log( "No object was tapped at " + gesture.Position );
		//	}
		//	Debug.Log( "Tap gesture detected at " + gesture.Position + 
		//		           ". It was sent by " + gesture.Recognizer.name );
		
	}
}
