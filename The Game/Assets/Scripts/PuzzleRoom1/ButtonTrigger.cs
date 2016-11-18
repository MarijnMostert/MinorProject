using UnityEngine;
using System.Collections;

public class ButtonTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		GameObject.Find ("WallGoingDown").GetComponent<DownMover> ().move = true;
	}

}
