using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CameraController : NetworkBehaviour {

	public GameObject[] targets;	//A list of targets to follow
	public Vector3 offset;
	public Vector3 cameraPosition;
	public float smoothTime = 0.1f;

	private Vector3 targetPosition;
	private Vector3 smoothDampVelocity;

	void Start () {
		targets = GameObject.FindGameObjectsWithTag ("Player");
		offset = new Vector3 (0f, 20f, -8f);
	}
	
	void FixedUpdate () {
		targets = GameObject.FindGameObjectsWithTag ("Player");
		targetPosition = getAveragePosition ();
		cameraPosition = targetPosition + offset;

		transform.position = cameraPosition;
	}

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
}
