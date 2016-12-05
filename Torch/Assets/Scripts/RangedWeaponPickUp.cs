using UnityEngine;
using System.Collections;

public class RangedWeaponPickUp : MonoBehaviour, IPickUp {

	public RangedWeapon weaponToEquip;

	//Equips the IceBall Weapon when picked up.
	public void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			GameObject player = other.gameObject;
			WeaponController playWeapController = player.GetComponent<WeaponController> ();
			playWeapController.Equip (weaponToEquip);
			player.GetComponent<Inventory> ().AddWeaponToInventory (weaponToEquip);
			Destroy (gameObject);
		}
	}
}
