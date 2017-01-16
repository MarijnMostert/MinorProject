using UnityEngine;
using System.Collections;

public class SetInactiveAudioSource : MonoBehaviour {

	[SerializeField] private AudioSource audioSource;

	void OnEnable(){
		if (audioSource.clip != null) {
			StartCoroutine (DeactivateAudioSource (audioSource.clip.length));
		}
	}

	IEnumerator DeactivateAudioSource(float waitingTime){
		yield return new WaitForSeconds (waitingTime);
		gameObject.SetActive (false);
	}

	void OnDisable(){
		StopAllCoroutines ();
	}
}
