using UnityEngine;
using System.Collections;

public class EndTutChest : MonoBehaviour {

	public GameObject chest;
	Chest chestScript;
	Doors thisdoor;

	// Use this for initialization
	void Start () {
		chestScript = chest.GetComponent<Chest> ();
		thisdoor = GetComponent<Doors> ();
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			if (chestScript.getUsed ()) {
				thisdoor.locked = false;
				thisdoor.Open ();
			}
		}
	}
}
