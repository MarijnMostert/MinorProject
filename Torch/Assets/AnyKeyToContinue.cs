using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AnyKeyToContinue : MonoBehaviour {

	void Update(){
		if (Input.anyKeyDown) {
			gameObject.SetActive (false);
			GameManager gameManager = GameManager.Instance;
			gameManager.audioSourceMusic.clip = gameManager.audioHomeScreen;
			gameManager.audioSourceMusic.Play ();
			gameManager.StartChoiceCanvas.SetActive (true);
		}
	}
}
