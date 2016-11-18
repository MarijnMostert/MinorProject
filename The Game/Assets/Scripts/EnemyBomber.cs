using UnityEngine;
using System.Collections;

public class EnemyBomber : Enemy {

	public BomberWeapon weapon;
	public LayerMask lookMask;

	private WeaponController weaponController;
	private float varDistanceToTarget;
	private GameObject weaponHolder;

	public Vector3 targetPosition;
	public Vector3 origin;
	public float distance;
	public float gravity;
	public float initialVelocity;
	public float angle;
	public float shootingAngleRadians;
	public float shootingAngleDegrees;

	new void Awake(){
		base.Awake ();
		weaponController = GetComponent<WeaponController> ();
	}

	// Use this for initialization
	new void Start () {
		base.Start ();
		weaponHolder = transform.FindChild ("Weapon Holder").gameObject;
		weapon = weaponController.currentWeapon as BomberWeapon;
		angle = weaponHolder.transform.eulerAngles.x;
	}

	void Update () {
		StartCoroutine (UpdatePath ());
		varDistanceToTarget = distanceToTarget ();
		if (target != null && varDistanceToTarget <= attackRange && (Time.time - lastAttackTime) > attackCooldown) {
			attack ();
		}
	}

	private float calculateAngle(float gravity, float distance, float initialVelocity){
		updateTargetPosition ();
		float angle = 1f / 2f * Mathf.Asin (gravity * distance / Mathf.Pow (initialVelocity, 2f));
		return angle;
	}

	private float angleRadianToDegrees(float angle){
		return angle * 360 / (2f * Mathf.PI);
	}

	private void attack(){
//		float force = Mathf.Sqrt (varDistanceToTarget * gravity / (Mathf.Sin (2f * (angle * 360 / (2 * Mathf.PI)))));
		float forcePart1 = varDistanceToTarget * gravity;
		float forcePart2 = Mathf.Sin (2f * (angle * 2f * Mathf.PI / 360));
		float forcePart3 = forcePart1 / forcePart2;
		float forcePart4 = Mathf.Abs (forcePart3);
		float forceFinal = Mathf.Sqrt (forcePart4);
		Debug.Log (forceFinal);
		weapon.force = forceFinal*45;
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

	private void updateTargetPosition(){
		targetPosition = target.transform.position;
	}

}
