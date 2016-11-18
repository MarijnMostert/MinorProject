using UnityEngine;
using System.Collections;

public class FireBallWeaponPickUp : PickUp, IPickUp {

	public GameObject fireBallWeapon;

	// Use this for initialization
	void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
		base.rotateY ();
	}

	//Equips the FireBall Weapon when picked up.
	public void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Player")){
			GameObject player = other.gameObject;
			PlayerWeaponController playWeapController = player.GetComponent<PlayerWeaponController>();
			playWeapController.Equip(fireBallWeapon);
			Destroy(gameObject);
		}
	}
}
