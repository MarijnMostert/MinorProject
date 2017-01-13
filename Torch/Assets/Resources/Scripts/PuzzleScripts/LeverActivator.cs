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

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile")) { 
				//SwitchLever (); 
		}
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			if (Input.GetButtonDown (interactionButton)) {
				SwitchLever ();
			}
		}
	}

	void SwitchLever () {
		is_on = !is_on;
	}

	void Update () {
		if (is_on) {
			handle.transform.rotation = Quaternion.RotateTowards (handle.transform.rotation, Quaternion.Euler (0, 0, -30), 1.5f);
		}
		else {
			handle.transform.rotation = Quaternion.RotateTowards (handle.transform.rotation, Quaternion.Euler (0, 0, 30), 1.5f);
		}
	}
		
	public void Deactivate(){
		gameObject.SetActive (false);
	}

	public void Activate(){
		gameObject.SetActive (true);
	}
}
