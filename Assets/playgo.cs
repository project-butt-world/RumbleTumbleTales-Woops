	using UnityEngine;
	using System.Collections;
	
	[RequireComponent(typeof(AudioSource))]
	public class playgo : MonoBehaviour {
		void Start() {
			audio.Play();
		}
	}
