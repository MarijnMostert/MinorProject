using UnityEngine;
using System.Collections;

public class EnemyGrunt : Enemy {

	NavMeshAgent agent;
	Animator animator;
	public bool attacknow;

	protected override void Awake(){
		base.Awake ();
	}

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		StartCoroutine (UpdatePath ());
		agent = GetComponent<NavMeshAgent> ();
		animator = GetComponent<Animator> ();
		attacknow = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameManager.enemyTarget != null && distanceToTarget () < attackRange && (Time.time - lastAttackTime) > attackCooldown) {
			attack ();
			attacknow = true;
		} else if(attacknow == true && ((Time.time - lastAttackTime) > (0.9f * attackCooldown))) {
			attacknow = false;
			if (animator != null) {
				Debug.Log ("set false");
				animator.SetBool ("Attack", false);
			}
		}
		if (animator != null) {
			if (agent.velocity.magnitude > 0.1f) {
				animator.SetBool ("Walk", true);
			} else {
				animator.SetBool ("Walk", false);
			}
		}
	}

	//If the player is close enough to the torch it will do damage
	private void attack(){
		if (animator != null) {
			Debug.Log ("jump");
			animator.SetBool ("Attack", true);
		}
		IDamagable damagableObject = gameManager.enemyTarget.GetComponent<IDamagable> ();
		damagableObject.takeDamage (attackDamage, false);
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
