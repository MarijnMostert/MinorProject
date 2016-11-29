using UnityEngine;
using System.Collections;

public class PlayerWeaponController : WeaponController {

	public int playerNumber;

	public string attackButton;
//	private string attackButtonController;

	void Awake(){
	}

	protected override void Start () {
		base.Start ();
		attackButton = "Attack" + playerNumber;
//		attackButtonController = "ControllerAttack" + playerNumber;
	}
	
	void Update () {
		if (Input.GetButton (attackButton)) {
			Attack ();
		}
	}

	private void Attack(){
		currentWeapon.Fire();
	}

	//OLD CONTROLLER INPUT. NOT USED ANYMORE
	/*
	private void fireController(){
		if (Input.GetButton (attackButtonController)) {
			if (currentWeapon.GetType ().Equals(typeof(RangedWeapon))) {
				RangedWeapon tempWeap = (RangedWeapon)currentWeapon;
				tempWeap.Fire();
			}
		}
	}
	*/
}
