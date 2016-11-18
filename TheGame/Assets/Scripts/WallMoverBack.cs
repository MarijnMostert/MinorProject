using UnityEngine;
using System.Collections;

public class WallMoverBack : MonoBehaviour {

	private Vector3 PlayerPos;
	
	// Update is called once per frame
	void Update () {
		PlayerPos = GameObject.Find("Player").transform.position;
		if (PlayerPos.x < -20.0f && PlayerPos.z > -20.0f) {
			transform.position = new Vector3 (0.0f, 2.0f, -20.0f);
		}	
	}
}
