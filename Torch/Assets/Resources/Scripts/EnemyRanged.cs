using UnityEngine;
using System.Collections;

public class EnemyRanged : Enemy {

	public LayerMask lookMask;

	[HideInInspector] public RangedWeapon weapon;
	private WeaponController weaponController;
	private float varDistanceToTarget;
	private float stoppingDistance = 0f;


	new void Awake(){
		base.Awake ();
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
		StartCoroutine (determineStoppingDistance ());
		varDistanceToTarget = 3.0f;//distanceToTarget ();
		if (gameManager.enemyTarget != null && varDistanceToTarget <= attackRange && (Time.time - lastAttackTime) > attackCooldown && canSeeTarget()) {
			attack ();
		}
	}

	private void attack(){
		if (weapon == null) {
			weapon = weaponController.currentWeapon as RangedWeapon;
		}
		weapon.Fire ();
		lastAttackTime = Time.time;
	}

	private bool canSeeTarget(){
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;

		Physics.Raycast (ray, out hit, attackRange, lookMask);

		if (hit.collider != null) {
			if (hit.collider.gameObject.Equals(gameManager.enemyTarget) || hit.collider.gameObject.CompareTag("Torch")) {
				return true;
			} 
		}
		return false;
	}

	IEnumerator determineStoppingDistance(){
		if (!canSeeTarget ()) {
			navMeshAgent.stoppingDistance = 0f;
		} else {
			navMeshAgent.stoppingDistance = stoppingDistance;
		}
		yield return new WaitForSeconds (0.2f);
	}

	private IEnumerator UpdatePath(){

		//First make sure there is a target
		while (gameManager.enemyTarget != null) {
			Vector3 targetPosition = new Vector3 (gameManager.enemyTarget.transform.position.x, 0, gameManager.enemyTarget.transform.position.z);

			//Set the target position for the Nav Mesh Agent
			navMeshAgent.SetDestination (targetPosition);
			transform.LookAt (new Vector3(targetPosition.x, transform.position.y, targetPosition.z));

			//Make sure that the Nav Mesh Agent refreshes not every frame (to spare costs)
			yield return new WaitForSeconds (refreshTime);
		}
	}
		
}