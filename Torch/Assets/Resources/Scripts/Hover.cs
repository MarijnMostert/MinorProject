using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {

	public float hoverAmplitudeX, hoverSpeedX;
	public float hoverAmplitudeY, hoverSpeedY;
	public float hoverAmplitudeZ, hoverSpeedZ;
	private float startingTime;

	void Start(){
		startingTime = Time.time;
	}

	// Update is called once per frame
	void Update () {
		float newX = transform.position.x + (1f / 100f * hoverAmplitudeX * Mathf.Sin ((Time.time - startingTime) * hoverSpeedX));
		float newY = transform.position.y + (1f / 100f * hoverAmplitudeY * Mathf.Sin ((Time.time - startingTime) * hoverSpeedY));
		float newZ = transform.position.z + (1f / 100f * hoverAmplitudeZ * Mathf.Sin ((Time.time - startingTime) * hoverSpeedZ));

		transform.position = new Vector3(newX, newY, newZ);
	}
}
