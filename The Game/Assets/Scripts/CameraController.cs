using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject[] targets;	//A list of targets to follow
	public Vector3 offset;
	public Vector3 cameraPosition;
	public float smoothTime = 0.1f;

	private Vector3 targetPosition;
	private Vector3 smoothDampVelocity;

	// Use this for initialization
	void Start () {
		targetPosition = getAveragePosition();
		offset = transform.position - targetPosition;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		targetPosition = getAveragePosition ();
		cameraPosition = targetPosition + offset;
		transform.position = Vector3.SmoothDamp (transform.position, cameraPosition, ref smoothDampVelocity, smoothTime);
	}

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
