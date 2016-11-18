using UnityEngine;
using System.Collections;

public class MonsterPlatformMover : MonoBehaviour {


	public float Speed;
	public float StopWhenZ;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		Vector3 Position = transform.position;
		while (Position.z > StopWhenZ) {
			Vector3 movement = new Vector3 (0.0f, 0.0f, -Speed);
			transform.Translate (movement * Time.deltaTime);
		}
		if(Position.y < -2.0f){
			Destroy(gameObject);
		}
	}
}