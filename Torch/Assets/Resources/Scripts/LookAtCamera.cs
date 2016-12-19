using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

	public Camera cam;
	public GameManager gameManager;

	void Start(){
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
		cam = gameManager.mainCamera;
	}

	void Update(){
		if (cam != null) {
			lookAtCamera (cam);
		}
	}

	void lookAtCamera(Camera cam){
		transform.rotation = cam.transform.rotation;
	}

}
