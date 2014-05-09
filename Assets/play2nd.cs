using UnityEngine;
using System.Collections;

public class play2nd : MonoBehaviour {

	// Use this for initialization
	void Start () {

		twosong ();
	
	}

	void twosong() {
		MasterAudio.PlaySound ("part2");

	}
}
