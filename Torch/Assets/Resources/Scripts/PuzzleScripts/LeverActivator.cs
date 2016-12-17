using UnityEngine;
using System.Collections;

public class LeverActivator : MonoBehaviour {

	private string interactionButton;
	public bool is_on = false;
	public GameObject handle;

	// Use this for initialization
	void Start () {
		interactionButton = "InteractionButton";
	}
	
	void OnTriggerStay(Collider other){
		if (other.gameObject.CompareTag ("Projectile")) { SwitchLever (); }
	
		if (other.gameObject.CompareTag ("Player")) {
			if (Input.GetButtonDown (interactionButton)) {
				SwitchLever ();
			}
		}
	}

	void SwitchLever () {
		if (is_on) {
			handle.transform.Rotate (0, 0, -60);
		}
		else {
			handle.transform.Rotate (0, 0, 60);
		}
		is_on = !is_on;
	}
		
}
