using UnityEngine;
using System.Collections;

public class MonsterPlatformMover2 : MonoBehaviour {


	public float Speed;
	public float StopWhenZ;
	public bool start = false;

	// Update is called once per frame
	void Update () {
		if (start == true) {
			Vector3 Position = transform.position;
			Vector3 movement = new Vector3 (0.0f, 0.0f, -Speed);
			transform.Translate (movement * Time.deltaTime);
			if (Position.z < StopWhenZ) {
				start = false;
			}
		}
	}
}