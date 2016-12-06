using UnityEngine;
using System.Collections;

public class DeadGround : MonoBehaviour {

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Torch")) {
			int currentH = other.GetComponent <Torch> ().health;
			other.GetComponent <Torch> ().takeDamage (currentH);
		}
	}

}
