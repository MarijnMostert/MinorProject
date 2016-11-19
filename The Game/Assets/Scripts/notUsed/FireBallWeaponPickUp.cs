using UnityEngine;
using System.Collections;

public class FireBallWeaponPickUp : PickUp, IPickUp {

	public Weapon fireBallWeapon;

	// Use this for initialization
	new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update ();
		base.rotateY ();
	}

	//Equips the FireBall Weapon when picked up.
	public void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Player")){
	//			GameObject player = other.gameObject;
	//		WeaponController playWeapController = player.GetComponent<WeaponController>();
	//		playWeapController.Equip(fireBallWeapon);
			Destroy(gameObject);
		}
	}
}
