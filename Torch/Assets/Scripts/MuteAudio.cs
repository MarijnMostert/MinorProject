using UnityEngine;
using System.Collections;

public class MuteAudio : MonoBehaviour {

	public bool audioMuted;
	public AudioSource audioSource;

	void Start(){
		audioSource = GameObject.Find ("Game Manager").GetComponent<AudioSource> ();
	}

	public void muteAudio(){
		if (audioMuted) {
			audioSource.mute = false;
			audioMuted = false;
		} else {
			audioSource.mute = true;
			audioMuted = true;
		}
	}
}
