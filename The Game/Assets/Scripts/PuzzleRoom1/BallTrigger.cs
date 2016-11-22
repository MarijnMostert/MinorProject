using UnityEngine;
using System.Collections;

public class BallTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		GameObject.Find ("BallWall").GetComponent<BallWallDown> ().Down();
	}

	void OnTriggerExit(Collider other){
		GameObject.Find ("BallWall").GetComponent<BallWallDown> ().Up();
	}
}
