using UnityEngine;
using System.Collections;

public class EnemyGrunt : Enemy {

	//private Animator anim;
	bool attack_anim;

	protected override void Awake(){
		base.Awake ();
		anim = GetComponent<Animator>();
		setAnim(anim);
	}

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		StartCoroutine (UpdatePath ());
	}
	
	// Update is called once per frame
	void Update () {
		if (!dead) {
			if (gameManager.enemyTarget != null && distanceToTarget () < attackRange && (Time.time - lastAttackTime) > attackCooldown) {
				attack ();
			}
		} else
		{
			anim.SetTrigger("Die");
		}
	}

	//If the player is close enough to the torch it will do damage
	private void attack(){
		IDamagable damagableObject = gameManager.enemyTarget.GetComponent<IDamagable> ();
		damagableObject.takeDamage (attackDamage, false);
		anim.SetTrigger("Attack");
		//Debug.Log (damagableObject);
		lastAttackTime = Time.time;
	}

	//A Coroutine for chasing a target
	private IEnumerator UpdatePath(){
		//First make sure there is a target
		while (gameManager.enemyTarget != null) {
			Vector3 targetPosition = new Vector3 (gameManager.enemyTarget.transform.position.x, 0, gameManager.enemyTarget.transform.position.z);

			//Set the target position for the Nav Mesh Agent
			navMeshAgent.SetDestination (targetPosition);

			//Make sure that the Nav Mesh Agent refreshes not every frame (to spare costs)
			yield return new WaitForSeconds (refreshTime);
		}
	}


}
