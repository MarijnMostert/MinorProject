using UnityEngine;
using System.Collections;

public class HealthPowerUp : PickUp, IPickUp {

	public int heal = 10;

	// Use this for initialization
	void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
		base.rotateY ();
	}

	//Heals the torch when picked up
	public void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			Torch torch = other.gameObject.GetComponentInChildren<Torch>();
			torch.heal (heal);
			Destroy (gameObject);
		}

	}
}
