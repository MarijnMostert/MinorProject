using UnityEngine;
using System.Collections;

public class backtoHomeScreen : MonoBehaviour {
	
	GameManager gameManager;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void OnTriggerEnter() {
		gameManager.TransitionDeathToMain ();
	}
}
