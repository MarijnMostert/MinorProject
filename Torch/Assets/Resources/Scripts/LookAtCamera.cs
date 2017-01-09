using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

	public Camera cam;
	private GameManager gameManager;
	public bool withManualOffset = false;
	public Vector3 Offset;

	void Start(){
		gameManager = GameManager.Instance;
		cam = gameManager.mainCamera;
	}

	void Update(){
		if (cam == null) {
			cam = gameManager.mainCamera;
		}

		if (cam != null) {
			lookAtCamera (cam);
		}
	}

	void lookAtCamera(Camera cam){
		transform.rotation = cam.transform.rotation;
		if (withManualOffset) {
			transform.position = transform.parent.position + Offset;
		}
	}

}
