using UnityEngine;
using System.Collections;

public class HealthPowerUp : PickUp, IPickUp {

	public int heal = 10;

	//Heals the torch when picked up
	public void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			Torch torch = other.gameObject.GetComponentInChildren<Torch>();
			torch.heal (heal);
			Destroy (gameObject);
		}

	}
}
