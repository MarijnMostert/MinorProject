using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerInteraction : NetworkBehaviour {

	private string interactionButton = "InteractionButton";
	private GameObject currentItem;

	void OnTriggerStay(Collider other){
		if (!isLocalPlayer) {
			return;
		}
		if (other.gameObject.CompareTag ("Interactable Item")||other.gameObject.CompareTag ("Torch")) {
			print ("collision detected with item");
			currentItem = other.gameObject;
			if (Input.GetButtonDown (interactionButton)) {
				print("input received #interact");
				currentItem.GetComponent<InteractableItem> ().action();
				//CmdInteract (currentItem);
			}
		}
	}
	/*[Command]
	void CmdInteract(GameObject currentItem){
		print("Command send");
		print(currentItem.GetType());
		currentItem.Invoke ();
	}*/
}