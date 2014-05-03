using UnityEngine;
using System.Collections;

public class DoStuffOnTap : MonoBehaviour {

	void OnTap( TapGesture gesture ) 
	{
		if( gesture.Selection )
			Debug.Log( "Tapped object: " + gesture.Selection.name );
		    this.transform.Translate(Vector2.up * Time.deltaTime, Space.World);
//		else
//			Debug.Log( "No object was tapped at " + gesture.Position );
//	}
	//	Debug.Log( "Tap gesture detected at " + gesture.Position + 
//		           ". It was sent by " + gesture.Recognizer.name );

}
}