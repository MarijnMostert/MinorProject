using UnityEngine;
using System.Collections;

public class EnemyRangedTraining : EnemyTraining {

	public RangedWeapon weapon;
	public LayerMask lookMask;

	private float stoppingDistance;
	public WeaponController weaponController;

	new void Awake(){
		base.Awake ();
		weaponController = GetComponent<WeaponController> ();
	}

	// Use this for initialization
	new void Start () {
		base.Start ();
		weapon = weaponController.currentWeapon as RangedWeapon;
		stoppingDistance = navMeshAgent.stoppingDistance;
		StartCoroutine (UpdatePath ());
	}
	
	void Update () {
		if (target != null  && (Time.time - lastAttackTime) > attackCooldown) {
			attack ();
		}
		if (distanceToTarget () < 2.5f) {
			transform.position = new Vector3 (0, 0, 0);
		}
	}

	private void attack(){
		if (weapon == null) {
			weapon = weaponController.currentWeapon as RangedWeapon;
		}
		if (Random.value > 0.2f) {
			weapon.Fire ();
		}
	}

	private bool canSeeTarget(){
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;

		Physics.Raycast (ray, out hit, attackRange, lookMask);

		if (hit.collider != null) {
			if (hit.collider.gameObject.Equals(target) || hit.collider.gameObject.CompareTag("BossShield").Equals(target)) {
				return true;
			} 
		}
		return false;
	}

	private void determineStoppingDistance(){
			navMeshAgent.stoppingDistance = stoppingDistance;
	}

	private IEnumerator UpdatePath(){

		//First make sure there is a target
		while (target != null && !dead) {
			Vector3 targetPosition = new Vector3 (target.transform.position.x, 0, target.transform.position.z);

			//Set the target position for the Nav Mesh Agent
			Vector3 rand = RandomMove();
			transform.LookAt (new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
			navMeshAgent.SetDestination (targetPosition + rand);

			//Make sure that the Nav Mesh Agent refreshes not every frame (to spare costs)
			yield return new WaitForSeconds (refreshTime);
		}
	}

	Vector3 RandomMove(){
		int radius = 10;
		Vector3 RandomDirection = Random.insideUnitSphere * radius;
		RandomDirection += transform.position;
		return RandomDirection;
	}
}
