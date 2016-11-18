using UnityEngine;
using System.Collections;

public class PlayerWeaponController : WeaponController {

	public int playerNumber;

	private string attackButton;

	new void Start () {
		base.Start ();
		attackButton = "Attack" + playerNumber;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton (attackButton)) {
			if (currentWeapon.GetType ().Equals(typeof(RangedWeapon))) {
				RangedWeapon tempWeap = (RangedWeapon)currentWeapon;
				tempWeap.fire();
			}
		}
	}
}
