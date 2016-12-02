using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class InteractableItem : NetworkBehaviour {
	public GameObject instructionPopUp;
	public float floatingHeight = 2f;

	[HideInInspector] 
	public Camera cam;
	private GameObject canvas;
	private string interactionButton;

	public virtual void Start () {
		canvas = Instantiate (instructionPopUp, new Vector3(transform.position.x, floatingHeight, transform.position.z), transform.rotation) as GameObject;
		canvas.SetActive (false);
		cam = Camera.main; 
		interactionButton = "InteractionButton";
	}

	void LateUpdate() {
		if (canvas != null) {
			if (cam == null) {
				cam = Camera.main;
			}
			lookAtCamera (canvas, cam);
		}
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.CompareTag ("Player")&&canvas!=null) {
			canvas.SetActive (true);
			if (Input.GetButtonDown (interactionButton)) {
				action (other.gameObject);
				Destroy (canvas);
			}
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			if (canvas != null) {
				canvas.SetActive (false);
			}
		}
	}

	void lookAtCamera(GameObject obj, Camera cam){
		obj.transform.rotation = cam.transform.rotation;
	}

	public virtual void action(GameObject triggerObject){
		Debug.Log ("Interactable item action triggered)");
	}
}
