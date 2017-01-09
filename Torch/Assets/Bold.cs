using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bold : MonoBehaviour {

	public Image speechImage;
	public Text speechText;

	public AudioClip[] clips;
	public AudioSource audioSource;
	public float chanceForClip = 0.01f;
	public float interval = 5f;

	void Start(){
		StartCoroutine (RandomSayings ());
	}

	IEnumerator RandomSayings(){
		while (true) {
			if (Random.value < chanceForClip) {
				audioSource.clip = clips [Random.Range (0, clips.Length)];
				audioSource.Play ();
			}
			yield return new WaitForSeconds (interval);
		}
	}
}
