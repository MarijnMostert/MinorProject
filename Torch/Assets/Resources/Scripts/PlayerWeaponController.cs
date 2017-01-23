using UnityEngine;
using System.Collections;

public class PlayerWeaponController : WeaponController {

	public int playerNumber;
	public WeaponInventory weaponInventory;

	private string attackButton;
//	private string attackButtonController;



    Animator anim;

	new void Start () {
		base.Start ();
		weaponInventory = UIInventory.Instance.weaponInventory;
		weaponInventory.AddWeaponToInventory (startingWeapon, playerNumber);
        anim = GetComponentInChildren<Animator>();
//		attackButtonController = "ControllerAttack" + playerNumber;
	}
    
    public void setNumber(int playerNumber)
    {
        this.playerNumber = playerNumber;
        attackButton = "Attack" + playerNumber;
    }

    void Update () {
		if (Input.GetButton (attackButton)||(Mathf.Abs(Input.GetAxis (attackButton))>0.1f) && !GameManager.Instance.GetTextFieldEnabled()) {
            if (anim != null)
            {
                anim.SetTrigger("attackTrigger");
            }
			Attack ();
		}
	}

	public void Attack(){
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
