using UnityEngine;
using System.Collections;

public class EnemyRanged : Enemy {

	public RangedWeapon weapon;

	private WeaponController weaponController;
//	private GameObject weaponHolder;
	private float distanceToTarget;

	new void Awake(){
		base.Awake ();
//		weaponHolder = gameObject.transform.FindChild ("Weapon Holder").gameObject;
		weaponController = GetComponent<WeaponController> ();
	}

	// Use this for initialization
	new void Start () {
		base.Start ();
		weapon = weaponController.currentWeapon as RangedWeapon;

	}
	
	void Update () {
		StartCoroutine (UpdatePath ());
		distanceToTarget = distanceToTorch ();
		if (target != null && distanceToTarget <= attackRange && (Time.time - lastAttackTime) > attackCooldown) {
			attack ();
		}
	}

	private void attack(){
		weapon.fire ();
	}

	private IEnumerator UpdatePath(){

		//First make sure there is a target
		while (target != null && !dead) {
			Vector3 targetPosition = new Vector3 (target.transform.position.x, 0, target.transform.position.z);

			//Set the target position for the Nav Mesh Agent
			navMeshAgent.SetDestination (targetPosition);
			transform.LookAt (new Vector3(targetPosition.x, transform.position.y, targetPosition.z));

			//Make sure that the Nav Mesh Agent refreshes not every frame (to spare costs)
			yield return new WaitForSeconds (refreshTime);
		}
	}
}
