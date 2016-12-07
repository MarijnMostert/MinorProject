using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

	private Camera cam;

	void Start(){
		cam = Camera.main;
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
