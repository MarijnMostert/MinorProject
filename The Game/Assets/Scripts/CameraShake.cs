using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	private Vector3 mainPosition;
	private Vector3 smoothDampVar;

	// Use this for initialization
	void Start () {
		mainPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetButton ("CameraShake")) {
			float currentTime = Time.time;
			for (float i = currentTime; i < currentTime + 0.7; i = i + 0.1f) {
				transform.position = Vector3.SmoothDamp (transform.position, transform.position + 0.2f*Random.insideUnitSphere, ref smoothDampVar, 0.1f);
			}
			transform.position = Vector3.SmoothDamp (transform.position, mainPosition, ref smoothDampVar, 0.1f);
		}
	}
}
