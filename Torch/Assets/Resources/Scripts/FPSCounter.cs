using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour {

	private Text text;
	private float deltaTime;
	private float fps;

	void Awake(){
		text = GetComponent<Text> ();
	}

	IEnumerator Start (){
		while (true) {
			if (Time.timeScale == 1) {
				yield return new WaitForSeconds (0.1f);
				fps = (1 / Time.deltaTime);
				text.text = "FPS: " + (Mathf.Round (fps));
			} else {
				text.text = "Pause";
			}
			yield return new WaitForSeconds (0.5f);
		}
	}


}
