using UnityEngine;
using System.Collections;

public class IceBallWeaponPickUp : PickUp, IPickUp {

	public GameObject iceBallWeapon;

	// Use this for initialization
	void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
		base.rotateY ();
	}

	//Equips the IceBall Weapon when picked up.
	public void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Player")){
			GameObject player = other.gameObject;
			PlayerWeaponController playWeapController = player.GetComponent<PlayerWeaponController>();
			playWeapController.Equip(iceBallWeapon);
			Destroy(gameObject);
		}
	}
}
