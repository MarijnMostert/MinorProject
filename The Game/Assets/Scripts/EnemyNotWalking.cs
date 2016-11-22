using UnityEngine;
using System.Collections;

public class EnemyNotWalking : Enemy {

	public RangedWeapon weapon;
	public LayerMask lookMask;

	private float stoppingDistance;
	private WeaponController weaponController;
	private float varDistanceToTarget;


	new void Awake(){
		base.Awake ();
		weaponController = GetComponent<WeaponController> ();
	}

	// Use this for initialization
	new void Start () {
		base.Start ();
		weapon = weaponController.currentWeapon as RangedWeapon;
	}

	void Update () {
		varDistanceToTarget = distanceToTarget ();
		if (target != null && varDistanceToTarget <= attackRange && (Time.time - lastAttackTime) > attackCooldown && canSeeTarget()) {
			attack ();
		}
	}

	private void attack(){
		weapon.fire ();
	}

	private bool canSeeTarget(){
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;

		Physics.Raycast (ray, out hit, attackRange, lookMask);

		if (hit.collider != null) {
			if (hit.collider.gameObject.Equals(target) || hit.collider.gameObject.CompareTag("Torch")) {
				return true;
			} 
		}
		return false;
	}

}
