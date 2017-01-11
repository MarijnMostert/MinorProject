using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pet : MonoBehaviour {

	public Image speechImage;
	public Text speechText;

	public AudioClip[] clips;
	public AudioSource audioSource;
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
				audioSource.clip = clips [Random.Range (0, clips.Length)];
				if (audioSource.clip != null) {
					audioSource.Play ();
				}
			}
			yield return new WaitForSeconds (interval);
		}
	}

	void OnDestroy(){
		StopAllCoroutines ();
	}
}
