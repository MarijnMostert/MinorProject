using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class Pet : AudioObject {

	public Image speechImage;
	public Text speechText;

	public AudioClip[] clips;
	public float chanceForClip = 0.01f;
	public float interval = 5f;

	void Start(){
		speechImage.gameObject.SetActive (false);
		speechText.text =  "";
		if (clips != null) {
			StartCoroutine (RandomSayings ());
		}
	}

	IEnumerator RandomSayings(){
		while (true) {
			if (Random.value < chanceForClip) {
				ObjectPooler.Instance.PlayAudioSource (clips [Random.Range (0, clips.Length)], mixerGroup, pitchMin, pitchMax, transform);
			}
			yield return new WaitForSeconds (interval);
		}
	}

	void OnDestroy(){
		StopAllCoroutines ();
	}
}
