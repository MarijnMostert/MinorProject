using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject[] targets;	//A list of targets to follow
	public Vector3 offset;
	private Vector3 targetPosition;

	// Use this for initialization
	void Start () {
		targetPosition = getAveragePosition();
		offset = transform.position - targetPosition;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		targetPosition = getAveragePosition();
		transform.position = targetPosition + offset;
	}

	private Vector3 getAveragePosition(){
		Vector3 temp = new Vector3(0,0,0);
		for (int i = 0; i < targets.Length; i++) {
			temp += targets[i].transform.position;
		}
		temp = temp / targets.Length;
		return temp;
	}
}
