using UnityEngine;
using System.Collections;

public class WallTorchHome : MonoBehaviour {

	public AudioSource audioSource;

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			for (int i = 0; i < 3; i++) {
				transform.GetChild(0).GetChild (i).gameObject.SetActive (true);

		    }
			if (audioSource != null) {
				audioSource.Play ();
			}
		}
	}

	void OnTriggerExit(Collider other){
        if (other.gameObject.CompareTag("Player")){
            for (int i = 0; i < 3; i++)
            {
               transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
            }
        }
		if (audioSource != null) {
			audioSource.Stop ();
		}
    }
}
