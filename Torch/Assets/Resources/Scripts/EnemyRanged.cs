using UnityEngine;
using System.Collections;

public class EnemyRanged : Enemy {

	public LayerMask lookMask;
	public float deviation = 20f;

	[HideInInspector] public RangedWeapon weapon;
	private WeaponController weaponController;
	private float varDistanceToTarget;
	private float stoppingDistance;

    private Animator anim;
    bool attack_anim;
    bool first_attack;

    new void Awake(){
		base.Awake ();
		weaponController = GetComponent<WeaponController> ();
	}

	// Use this for initialization
	new void OnEnable () {
		base.OnEnable ();
		weapon = weaponController.currentWeapon as RangedWeapon;
        anim = GetComponent<Animator>();
        setAnim(anim);
		stoppingDistance = navMeshAgent.stoppingDistance;
		StartCoroutine (UpdatePath ());
	}
	
	void Update () {
		determineStoppingDistance ();
		varDistanceToTarget = distanceToTarget ();
		if (Time.realtimeSinceStartup > .5f && gameManager.enemyTarget != null && varDistanceToTarget <= attackRange && canSeeTarget()) {
            anim.SetBool("attack", true);
            if ((Time.time - lastAttackTime) > attackCooldown) {
                StartCoroutine(attack());
            }
		} else
        {
            anim.SetBool("attack", false);
            first_attack = true;
            attack_anim = false;
        }
	}

	private IEnumerator attack(){
        if (first_attack)
        {
            yield return new WaitForSeconds(.5f);
        }
        yield return new WaitForSeconds(.1f);
		if (weapon == null) {
			weapon = weaponController.currentWeapon as RangedWeapon;
		}

		//Apply a deviation to the aiming of the enemy
		Vector3 weaponRotOriginal = weapon.gameObject.transform.eulerAngles;
		Vector3 newWeaponRot = weaponRotOriginal + new Vector3 (0f, Random.Range (-deviation, deviation), 0f);
		weapon.gameObject.transform.eulerAngles = newWeaponRot;
		weapon.Fire ();
		weapon.gameObject.transform.eulerAngles = weaponRotOriginal;

		lastAttackTime = Time.time;
        first_attack = false;
        yield return null;
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

	private void determineStoppingDistance(){
		if (!canSeeTarget ()) {
			navMeshAgent.stoppingDistance = 0f;
		} else {
			navMeshAgent.stoppingDistance = stoppingDistance;
		}
	}

	private IEnumerator UpdatePath(){

		//First make sure there is a target
		while (gameManager.enemyTarget != null) {
			Vector3 targetPosition = new Vector3 (gameManager.enemyTarget.transform.position.x, 0, gameManager.enemyTarget.transform.position.z);

			//Set the target position for the Nav Mesh Agent
			if (navMeshAgent.enabled) {
				navMeshAgent.SetDestination (targetPosition);
				transform.LookAt (new Vector3 (targetPosition.x, transform.position.y, targetPosition.z));
			}

			//Make sure that the Nav Mesh Agent refreshes not every frame (to spare costs)
			yield return new WaitForSeconds (refreshTime);
		}
	}
		
}