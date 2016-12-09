using UnityEngine;
using System.Collections;

public class Toggler : MonoBehaviour {

	public string button;
	public bool isActive = false;

	void Start(){
		gameObject.SetActive (isActive);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Pause")) {
			if (isActive) {
				gameObject.SetActive (false);
				isActive = false;
			} else {
				gameObject.SetActive (true);
				isActive = true;
			}
		}
	}
}
