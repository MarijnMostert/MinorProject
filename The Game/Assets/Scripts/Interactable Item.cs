using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class InteractableItem : NetworkBehaviour {
	public GameObject instructionPopUp;
	public float floatingHeight = 2f;

	private Camera cam;
	private GameObject canvas;
	private string interactionButton = "InteractionButton";

	public virtual void Start () {
		canvas = Instantiate (instructionPopUp, new Vector3(transform.position.x, floatingHeight, transform.position.z), transform.rotation) as GameObject;
		canvas.SetActive (false);
		cam = Camera.main; 
	}

	void LateUpdate() {
		lookAtCamera (canvas, cam);
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			canvas.SetActive (true);
			if (Input.GetButtonDown (interactionButton)) {
				action ();
				canvas.SetActive (false);
			}
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			canvas.SetActive (false);
		}
	}

	void lookAtCamera(GameObject obj, Camera cam){
		obj.transform.rotation = cam.transform.rotation;
	}

	public virtual void action(){
		Debug.Log ("Interactable item action triggered");
	}
}
