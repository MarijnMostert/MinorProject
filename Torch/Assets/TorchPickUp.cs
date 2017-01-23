using UnityEngine;
using System.Collections;

public class TorchPickUp : InteractableItem {

	public bool equipped = false;

	public override void Start () {
		base.Start (); 
		canvas.SetActive (true);

		//If the number of players is 1, the torch is automatically picked up by player 1
		if (gameManager.numberOfPlayers == 1) {
			pickUpTorch (gameManager.playerManagers [0].playerInstance);
		}
	}
	
	void Update () {
		if (Input.GetButtonDown("DropTorch1") && equipped && gameManager.playerManagers[0].playerInstance.GetComponentInChildren<Torch>() != null) {
			releaseTorch ();
		}
		if (Input.GetButtonDown ("DropTorch2") && equipped && gameManager.playerManagers [1].playerInstance.GetComponentInChildren<Torch>() != null) {
			releaseTorch ();
		}
	}

	public override void action(GameObject triggerObject){
		pickUpTorch (triggerObject);
	}

	void pickUpTorch(GameObject triggerObject){
		Debug.Log ("Torch is picked up");
		gameManager.WriteTorchPickup ();
		transform.parent.SetParent (triggerObject.transform.FindChild("Torch Holder"));
		transform.parent.position = transform.parent.parent.position;
		transform.parent.rotation = transform.parent.parent.rotation;
		gameManager.enemyTarget = triggerObject;
		gameManager.camTarget = triggerObject;
		canvas.SetActive (false);
		equipped = true;
	}

	public void releaseTorch(){
		activated = false;
		Debug.Log ("Torch is dropped");
		transform.parent.parent = null;
		gameManager.enemyTarget = transform.parent.gameObject;
		gameManager.camTarget = transform.parent.gameObject;
		equipped = false;
		canvas.transform.position = new Vector3 (transform.position.x, canvasFloatingHeight, transform.position.z);
		canvas.SetActive (true);
	}
		
	protected override void OnTriggerStay(Collider other){
		if (other.gameObject.CompareTag ("Player")&&canvas!=null) {
			if (Input.GetButtonDown (interactionButton)) {
				action (other.gameObject);
				canvas.gameObject.SetActive (false);
			}
		}
	}

	protected override void OnTriggerExit(Collider other){

	}
}
