using UnityEngine;
using System.Collections;

public class PowerUpPickUp : MonoBehaviour, IPickUp {

	public GameObject PowerUp;

	public void OnTriggerEnter(Collider collider){
		if(collider.gameObject.CompareTag("Player")){
			PowerUpInventory powerUpInventory = collider.GetComponent<PowerUpInventory> ();
			if (!powerUpInventory.isFull ()) {
				powerUpInventory.AddItemToInventory (PowerUp);
				transform.parent.gameObject.SetActive (false);
			}
		}
	}
}
