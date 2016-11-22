using UnityEngine;
using System.Collections;

public class ButtonTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if (GameObject.Find ("WallGoingDown") != null) {
			GameObject.Find ("WallGoingDown").GetComponent<DownMover> ().move = true;
		}
	}

}
