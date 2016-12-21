using UnityEngine;
using System.Collections;

public class RangedWeaponPickUp : MonoBehaviour, IPickUp {

	public Weapon weaponToEquip;

	//Equips the IceBall Weapon when picked up.
	public void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			GameObject player = other.gameObject;
			PlayerWeaponController playWeapController = player.GetComponent<PlayerWeaponController> ();
			playWeapController.Equip (weaponToEquip);


			/*
			if (GameObject.Find ("Game Manager").GetComponent<GameManager> ().inventory.GetComponent<Inventory>().AddWeaponToInventory (weaponToEquip, playWeapController.playerNumber)) {
				playWeapController.Equip (weaponToEquip);
			}
			*/
			/*
			if (player.GetComponent<Inventory> ().AddWeaponToInventory (weaponToEquip)) {
				playWeapController.Equip (weaponToEquip);
			}
			*/
			Destroy (gameObject);
		}
	}
}
