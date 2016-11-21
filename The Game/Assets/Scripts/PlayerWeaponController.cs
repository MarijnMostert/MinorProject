using UnityEngine;
using System.Collections;

public class PlayerWeaponController : WeaponController {

	public int playerNumber;

	private string attackButtonMouse;
	private string attackButtonController;
	private PlayerPrefsManager playerPrefsManager;

	void Awake(){
		playerPrefsManager = GameObject.Find ("SceneManager").GetComponent<PlayerPrefsManager> ();
	}

	new void Start () {
		base.Start ();
		attackButtonMouse = "Attack" + playerNumber;
		attackButtonController = "ControllerAttack" + playerNumber;
	}
	
	// Update is called once per frame
	void Update () {
		if (playerPrefsManager.controllerInput == 0) {
			fireMouse ();
		} else if (playerPrefsManager.controllerInput == 1) {
			fireController ();
		}
	}

	private void fireMouse(){
		if (Input.GetButton (attackButtonMouse)) {
			if (currentWeapon.GetType ().Equals(typeof(RangedWeapon))) {
				RangedWeapon tempWeap = (RangedWeapon)currentWeapon;
				tempWeap.fire();
			}
		}

	}

	private void fireController(){
		if (Input.GetButton (attackButtonController)) {
			if (currentWeapon.GetType ().Equals(typeof(RangedWeapon))) {
				RangedWeapon tempWeap = (RangedWeapon)currentWeapon;
				tempWeap.fire();
			}
		}

	}
}
