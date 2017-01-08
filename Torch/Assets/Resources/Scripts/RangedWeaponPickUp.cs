using UnityEngine;
using System.Collections;

public class RangedWeaponPickUp : MonoBehaviour, IPickUp {

	public Weapon weaponToEquip;

	//Equips the IceBall Weapon when picked up.
	public void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			
			GameObject player = other.gameObject;
			WeaponController playWeapController = player.GetComponent<WeaponController> ();
			int playerNumber = player.GetComponent<PlayerMovement> ().playerNumber;
			WeaponInventory weaponInventory = UIInventory.Instance.weaponInventory;

			if (weaponInventory.AddWeaponToInventory (weaponToEquip, playerNumber)) {
				playWeapController.Equip (weaponToEquip);
			}
			Destroy (transform.parent.gameObject);
		}
	}
}
