using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject target;
	public Vector3 offset;
	public Vector3 rotation;
	public float smoothTime = 0.1f;

	private Vector3 targetPosition;
	private Vector3 smoothDampVelocity;
	private Vector3 cameraPosition;
	public GameManager gameManager;

	void Start(){
	}

	void FixedUpdate () {
		//targetPosition = getAveragePosition ();
		targetPosition = gameManager.camTarget.transform.position;
		cameraPosition = targetPosition + offset;

		//The smoothdamp makes sure the camera follows the target(s) smoothly
		transform.position = Vector3.SmoothDamp (transform.position, cameraPosition, ref smoothDampVelocity, smoothTime);
		transform.eulerAngles = rotation;
	}
		
	/*
	//Calculate the average position between all targets (players).
	private Vector3 getAveragePosition(){
		Vector3 temp = new Vector3(0,0,0);
		for (int i = 0; i < targets.Length; i++) {
			if (targets [i] != null) {
				temp += targets [i].transform.position;
			}
		}
		temp = temp / targets.Length;
		return temp;
	}

	*/
}
