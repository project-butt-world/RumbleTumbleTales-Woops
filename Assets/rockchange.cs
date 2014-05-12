using UnityEngine;
using System.Collections;

public class rockchange : MonoBehaviour {



	void OnMouseDown() {
		AutoFade.LoadLevel (3, 3, 1, Color.black);
	}
}
