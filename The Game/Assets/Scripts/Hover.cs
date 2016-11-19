using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {

	public float hoverAmplitude = 2f;
	public float hoverSpeed = 2f;

	// Update is called once per frame
	void Update () {
		float newY = transform.position.y + (1f / 100f * hoverAmplitude * Mathf.Sin (Time.time * hoverSpeed));
		transform.position = new Vector3(transform.position.x, newY, transform.position.z);
	}
}
