using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class InteractableItem : AudioObject {

	protected GameManager gameManager;
	
	public float canvasFloatingHeight = 2f;
	[HideInInspector] public Camera cam;
	public GameObject canvas;
	protected string interactionButton = "InteractionButton";
	[SerializeField] protected bool multiUsable = false;
	[SerializeField] protected bool activated = false;

	public virtual void Start () {
		gameManager = GameManager.Instance;
		canvas.gameObject.SetActive (false);
		cam = Camera.main; 
		interactionButton = "InteractionButton";
	}

	void LateUpdate() {
		if (canvas != null) {
			if (cam == null) {
				cam = GameManager.Instance.mainCamera;
			}
		}
	}

	protected virtual void OnTriggerStay(Collider other){
		if (other.gameObject.CompareTag ("Player") && canvas!=null && !activated) {
			canvas.SetActive (true);
			if (Input.GetButtonDown (interactionButton)) {
				action (other.gameObject);
				canvas.gameObject.SetActive (false);
				if (!multiUsable) {
					activated = true;
				}
			}
		}
	}

	protected virtual void OnTriggerExit(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			if (canvas != null) {
				canvas.SetActive (false);
			}
		}
	}

	public virtual void action(GameObject triggerObject){
		Debug.Log ("Interactable item action triggered)");
	}
}
