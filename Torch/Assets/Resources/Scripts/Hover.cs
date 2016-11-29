using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {

	public float hoverAmplitudeX, hoverSpeedX;
	public float hoverAmplitudeY, hoverSpeedY;
	public float hoverAmplitudeZ, hoverSpeedZ;

	// Update is called once per frame
	void Update () {
		float newX = transform.position.x + (1f / 100f * hoverAmplitudeX * Mathf.Sin (Time.time * hoverSpeedX));
		float newY = transform.position.y + (1f / 100f * hoverAmplitudeY * Mathf.Sin (Time.time * hoverSpeedY));
		float newZ = transform.position.z + (1f / 100f * hoverAmplitudeZ * Mathf.Sin (Time.time * hoverSpeedZ));

		transform.position = new Vector3(newX, newY, newZ);
	}
}
