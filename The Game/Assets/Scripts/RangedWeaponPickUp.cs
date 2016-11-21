using UnityEngine;
using System.Collections;

public class RangedWeaponPickUp : PickUp, IPickUp {

	public RangedWeapon weaponToEquip;

	// Use this for initialization
	new void Start () {
		base.Start ();
	}

	// Update is called once per frame
	new void Update () {
		base.Update ();
		base.rotateY ();
	}

	//Equips the IceBall Weapon when picked up.
	public void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			GameObject player = other.gameObject;
			WeaponController playWeapController = player.GetComponent<WeaponController> ();
			playWeapController.Equip (weaponToEquip);
			Destroy (gameObject);
		}
	}
}
