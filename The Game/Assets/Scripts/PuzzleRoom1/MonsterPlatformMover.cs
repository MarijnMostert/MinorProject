using UnityEngine;
using System.Collections;

public class MonsterPlatformMover : MonoBehaviour {


	public float Speed;
	public float StopWhenZ;
	public bool start = false;

	// Update is called once per frame
	void Update () {
		if (start == true) {
			Vector3 Position = transform.position;
			Vector3 movement = new Vector3 (0.0f, 0.0f, -Speed);
			transform.Translate (movement * Time.deltaTime);
			if (Position.z < 10.0f) {
				GameObject.Find ("MonsterPlatforms2").GetComponent<MonsterPlatformMover2> ().start = true;
			}
			if (Position.z < StopWhenZ) {
				start = false;
			} 
		}
	}
}