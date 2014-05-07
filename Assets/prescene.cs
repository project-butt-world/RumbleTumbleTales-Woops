using UnityEngine;
using System.Collections;

public class prescene : MonoBehaviour {

	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnLevelWasLoaded(int level) {
		if (level == 2)
			print("Woohoo");
		
	}
}
