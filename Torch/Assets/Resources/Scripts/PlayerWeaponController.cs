using UnityEngine;
using System.Collections;

public class PlayerWeaponController : WeaponController {

	public int playerNumber;

	private string attackButton;
//	private string attackButtonController;

	void Awake(){
	}

	new void Start () {
		base.Start ();
//		attackButtonController = "ControllerAttack" + playerNumber;
	}
    
    public void setNumber(int playerNumber)
    {
        this.playerNumber = playerNumber;
        attackButton = "Attack" + playerNumber;
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
