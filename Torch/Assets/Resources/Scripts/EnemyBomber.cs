using UnityEngine;
using System.Collections;

public class EnemyBomber : Enemy {

	public BomberWeapon weapon;
	public LayerMask lookMask;

	private WeaponController weaponController;
	private float varDistanceToTarget;
	private GameObject weaponHolder;

	private Vector3 targetPosition;
	public float gravity;
	//public float initialVelocity;
	private float angle;
    //private Vector3 prevPosition;

    //private Animator anim;
    bool attack_anim;

    float starttime;

	new void Awake(){
		base.Awake ();
		weaponController = GetComponent<WeaponController> ();
        anim = GetComponent<Animator>();
        starttime = Time.time;
	}

	// Use this for initialization
	new void OnEnable () {
		if (!firstTimeActive || InstantiatedByObjectPooler) {
			base.OnEnable ();
			weaponHolder = transform.FindChild ("Weapon Holder").gameObject;
			weapon = weaponController.currentWeapon as BomberWeapon;
			angle = weaponHolder.transform.eulerAngles.x;
			//	prevPosition = target.transform.position;
			StartCoroutine (UpdatePath ());
		} else {
			navMeshAgent.enabled = false;
			firstTimeActive = false;
		}
	}

    void Update()
    {
        if (Time.time > starttime + 1.02)
        {
            if (!dead)
            {
                varDistanceToTarget = distanceToTarget();
                anim.SetBool("attack", false);
                if (Time.realtimeSinceStartup > .5f && gameManager.enemyTarget != null && varDistanceToTarget <= attackRange && (Time.time - lastAttackTime) > attackCooldown)
                {
                    StartCoroutine(attack());
                }
            }
            else
            {
                anim.SetBool("dead", true);
            }
        }
	}

    private IEnumerator attack()
    {
        //		varDistanceToTarget = futureTargetPosition ();
        //		float force = Mathf.Sqrt (varDistanceToTarget * gravity / (Mathf.Sin (2f * (angle * 360 / (2 * Mathf.PI)))));
                  
        anim.SetBool("attack", true);
        yield return new WaitForSeconds(.8f);
        float forcePart1 = varDistanceToTarget * gravity;
        float forcePart2 = Mathf.Sin(2f * (angle * 2f * Mathf.PI / 360));
        float forcePart3 = forcePart1 / forcePart2;
        float forcePart4 = Mathf.Abs(forcePart3);
        float forceFinal = Mathf.Sqrt(forcePart4);
        if (weapon == null)
        {
            weapon = weaponController.currentWeapon as BomberWeapon;
        }
        weapon.force = forceFinal * 45;
        weapon.fire();
        yield return null;
    }

	private IEnumerator UpdatePath(){

		//First make sure there is a target
		while (gameManager.enemyTarget != null) {
			targetPosition = new Vector3 (gameManager.enemyTarget.transform.position.x, 0, gameManager.enemyTarget.transform.position.z);

			//Set the target position for the Nav Mesh Agent
			if (navMeshAgent.enabled) {
				navMeshAgent.SetDestination (targetPosition);
				transform.LookAt (new Vector3 (targetPosition.x, transform.position.y, targetPosition.z));
			}

			//Make sure that the Nav Mesh Agent refreshes not every frame (to spare costs)
			yield return new WaitForSeconds (refreshTime);
		}
	}

	private void updateTargetPosition(){
		targetPosition = gameManager.enemyTarget.transform.position;
	}

/*	private Vector3 futureTargetPosition(){
		Vector3 direction = target.transform.position - prevPosition;
		Vector3 futureTargetPosition = target.transform.position + 10f * direction;
		return futureTargetPosition;
	}
*/
}
