using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

	private GameObject obj;
	private Camera cam;

	void Start(){
		obj = gameObject;
		cam = Camera.main;
	}

	void Update(){
		lookAtCamera (obj, cam);
	}

	void lookAtCamera(GameObject obj, Camera cam){
		obj.transform.rotation = cam.transform.rotation;
	}
}
