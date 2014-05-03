using UnityEngine;
using System.Collections;

public class TapTutorial : MonoBehaviour {



		void OnTap( TapGesture gesture ) 
	{
		if( gesture.Selection )
			Debug.Log( "Tapped object: " + gesture.Selection.name );
		else
			Debug.Log( "No object was tapped at " + gesture.Position );
	}

}
