using UnityEngine;
using System.Collections;

public class BlockMover : MonoBehaviour {

	public float speed;
		
	// Update is called once per frame
	void Update () {
		Vector3 movement = new Vector3 (0.0f, 0.0f, -speed);
		transform.Translate (movement * Time.deltaTime);
	}
}
