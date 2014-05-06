using UnityEngine;


public class Alarm : MonoBehaviour
{
	public AudioClip alarmSound;


	public void OnGetHelp()
	{
		audio.PlayOneShot(alarmSound);
	}
}