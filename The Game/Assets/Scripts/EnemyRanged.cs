using UnityEngine;
using System.Collections;

public class EnemyRanged : Enemy {

	public RangedWeapon weapon;
	public LayerMask lookMask;

	private float stoppingDistance;
	private WeaponController weaponController;
//	private GameObject weaponHolder;
	private float varDistanceToTarget;


	new void Awake(){
		base.Awake ();
//		weaponHolder = gameObject.transform.FindChild ("Weapon Holder").gameObject;
		weaponController = GetComponent<WeaponController> ();
	}

	// Use this for initialization
	new void Start () {
		base.Start ();
		weapon = weaponController.currentWeapon as RangedWeapon;
		stoppingDistance = navMeshAgent.stoppingDistance;

	}
	
	void Update () {
		StartCoroutine (UpdatePath ());
		determineStoppingDistance ();
		varDistanceToTarget = distanceToTarget ();
		if (target != null && varDistanceToTarget <= attackRange && (Time.time - lastAttackTime) > attackCooldown && canSeeTarget()) {
			attack ();
		}
	}

	private void attack(){
		if (weapon == null) {
			weapon = weaponController.currentWeapon as RangedWeapon;
		}
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

	private void determineStoppingDistance(){
		if (!canSeeTarget ()) {
			navMeshAgent.stoppingDistance = 0f;
		} else {
			navMeshAgent.stoppingDistance = stoppingDistance;
		}
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
