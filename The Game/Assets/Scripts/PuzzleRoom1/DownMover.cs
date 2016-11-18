using UnityEngine;
using System.Collections;

public class DownMover : MonoBehaviour {

	public float wallSpeed;
	public float DestroyWhenY;
	public bool move = false;
	private Vector3 Position;
	
	// Update is called once per frame
	void Update () {
		if (move == true) {
			Position = transform.position;
			Vector3 movement = new Vector3 (0.0f, -wallSpeed, 0.0f);
			transform.Translate (movement * Time.deltaTime);
			if (Position.y < DestroyWhenY) {
				Destroy (gameObject);
			}
		}
	}
}
