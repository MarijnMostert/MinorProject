using UnityEngine;
using System.Collections;

public class PlayerWeaponController : WeaponController {

	public int playerNumber;
	public Inventory inventory;

	private string attackButton;
//	private string attackButtonController;

	public AudioClip clip_attack;
	private AudioSource audioSource;

    Animator anim;

	void Awake(){
		audioSource = GetComponent<AudioSource> ();
	}

	new void Start () {
		base.Start ();
		inventory = gameObject.GetComponent<Inventory> ();
		inventory.AddWeaponToInventory (startingWeapon);
        anim = GetComponentInChildren<Animator>();
//		attackButtonController = "ControllerAttack" + playerNumber;
	}
    
    public void setNumber(int playerNumber)
    {
        this.playerNumber = playerNumber;
        attackButton = "Attack" + playerNumber;
    }

    void Update () {
		if (Input.GetButton (attackButton)) {
            if (anim != null)
            {
                anim.SetBool("Attack",true);
                anim.SetBool("Attack", false);
            }
			Attack ();
		}
	}

	private void Attack(){
		currentWeapon.Fire();
		if (clip_attack != null) {
			audioSource.clip = clip_attack;
			audioSource.pitch = Random.Range (0.8f, 1.1f);
			audioSource.Play ();
		}
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
