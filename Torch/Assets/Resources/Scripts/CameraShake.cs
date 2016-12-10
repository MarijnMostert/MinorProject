using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	public float shakeLength = 0.7f;
	public float intensity = 0.2f;
	public float iterationTime = 0.1f;

	private Vector3 mainPosition;
	private Vector3 smoothDampVar;

	void Start () {
		mainPosition = transform.position;
	}

	public void cameraShake(float shakeLength, float intensity, float iterationTime){
		float currentTime = Time.time;
		for (float i = currentTime; i < currentTime + shakeLength; i = i + iterationTime) {
			transform.position = Vector3.SmoothDamp (transform.position, transform.position + intensity * Random.insideUnitSphere, ref smoothDampVar, iterationTime);
		}
		transform.position = Vector3.SmoothDamp (transform.position, mainPosition, ref smoothDampVar, iterationTime);
	}
}
