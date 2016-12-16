using UnityEngine;
using System.Collections;

public class LeverActivator : MonoBehaviour {

	private string interactionButton;
	public bool is_on = false;
	public bool SwitchGround;
	public bool EndPuzzle;
	Vector3 center;
	GameObject bridges;
	bool made = false;

	// Use this for initialization
	void Start () {
		interactionButton = "InteractionButton";
	}
	
	void OnTriggerStay(Collider other){
		if (SwitchGround && other.gameObject.CompareTag ("Projectile")) { GetComponent<SwitchGround> ().rotateGround (); }
	
		if ((EndPuzzle || SwitchGround) && other.gameObject.CompareTag ("Player")) {
			if (Input.GetButtonDown (interactionButton)) {
				SwitchLever ();
				if (!made && EndPuzzle) { EndThisPuzzle (); }
				if (SwitchGround) { GetComponent<SwitchGround> ().rotateGround (); }
			}
		}
	}

	public void SetCenter (Vector3 center) {
		this.center = center;
	}

	void EndThisPuzzle () {
		made = true;
		bridges = (GameObject)Resources.Load ("Prefabs/Bridges", typeof(GameObject));
		GameObject instantiated = Instantiate (bridges);
		instantiated.transform.position = center;
		OpenDoors ();
	}

	void OpenDoors () {}

	void SwitchLever () {
		GetComponentInChildren<SwitchLever> ().flipSwitch ();
		is_on = !is_on;
	}
}
