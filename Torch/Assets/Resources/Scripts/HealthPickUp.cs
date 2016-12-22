using UnityEngine;
using System.Collections;

public class HealthPickUp : MonoBehaviour, IPickUp {

	public int healAmount = 10;

	//Heals the torch when picked up
	public void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			GameObject.Find ("Game Manager").GetComponent<GameManager> ().torch.heal (healAmount);
			Destroy (transform.parent.gameObject);
		}
	}
}
