using UnityEngine;
using System.Collections;

public class PingPongPlatform : MonoBehaviour {

	public char choice; //x y z
	public float amplitude;
	public float speed;
	public float offset = 0f;

	private Vector3 startingPos;
	private float useAmp;

	void Start () {

		offset = (float) Random.Range (1, 9);
		speed = Random.Range (2.0f, 3.0f);

		startingPos = transform.position;
		useAmp = amplitude / speed;
	}

	// Update is called once per frame
	void Update () {
		float newPos = speed * (Mathf.PingPong (Time.time + offset, 2*useAmp) - useAmp);
		switch (choice) {
		case 'x':
			transform.position = startingPos + new Vector3 (newPos, 0f, 0f);
			break;
		case 'y':
			transform.position = startingPos + new Vector3 (0f, newPos, 0f);
			break;
		case 'z':
			transform.position = startingPos + new Vector3 (0f, 0f, newPos);
			break;
		}
	}
}
