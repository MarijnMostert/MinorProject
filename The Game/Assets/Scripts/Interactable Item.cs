using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class InteractableItem : NetworkBehaviour {
	public GameObject instructionPopUp;
	public float floatingHeight = 2f;

	private Camera cam;
	private GameObject canvas;
	private string interactionButton;

	void Start () {
		canvas = Instantiate (instructionPopUp, new Vector3(transform.position.x, floatingHeight, transform.position.z), transform.rotation) as GameObject;
		canvas.SetActive (false);
		cam = Camera.main; 
		interactionButton = "InteractionButton";
	}

	void LateUpdate() {
		lookAtCamera (canvas, cam);
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			canvas.SetActive (true);
			if (Input.GetButtonDown (interactionButton)) {
				action ();
				Destroy (gameObject);
				Destroy (canvas);
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
		Debug.Log ("Interactable item action triggered)");
	}
}
