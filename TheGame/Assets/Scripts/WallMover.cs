using UnityEngine;
using System.Collections;

public class WallMover : MonoBehaviour {

	public float wallSpeed;

	public float maxX;

	// Update is called once per frame
	void Update () {
		Vector3 Position = transform.position;
		Vector3 movement = new Vector3 (wallSpeed, 0.0f, 0.0f);
		transform.Translate (movement * Time.deltaTime);
	}
}
