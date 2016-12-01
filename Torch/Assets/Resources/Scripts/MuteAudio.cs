using UnityEngine;
using System.Collections;

public class MuteAudio : MonoBehaviour {

	public bool audioMuted;
	public AudioSource audioSource;
    public GameObject game_manager;

	void Start(){
		audioSource = game_manager.GetComponent<AudioSource> ();
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
