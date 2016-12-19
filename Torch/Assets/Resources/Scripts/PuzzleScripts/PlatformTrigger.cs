using UnityEngine;
using System.Collections;

public class PlatformTrigger : MonoBehaviour {

	public bool entered = false;
	public bool stayed = false;
	public GameObject myParent;

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			entered = true;
			stayed = true;			
			other.gameObject.transform.SetParent (myParent.transform);
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			stayed = false;
			myParent.transform.DetachChildren ();
		}
	}
}




































