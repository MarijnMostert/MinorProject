using UnityEngine;
using System.Collections;

public class EnemyGrunt : Enemy {

	new void Awake(){
		base.Awake ();
	}

	// Use this for initialization
	new void Start () {
		base.Start ();
		StartCoroutine (UpdatePath ());

	}
	
	// Update is called once per frame
	void Update () {
<<<<<<< HEAD:The Game/Assets/Resources/Scripts/EnemyGrunt.cs
		if (target != null && (Time.time - lastAttackTime) > attackCooldown) {
=======
		if (gameManager.enemyTarget != null && distanceToTarget () < attackRange && (Time.time - lastAttackTime) > attackCooldown) {
>>>>>>> master:Torch/Assets/Resources/Scripts/EnemyGrunt.cs
			attack ();
		}
	}

	//If the player is close enough to the torch it will do damage
	private void attack(){
<<<<<<< HEAD:The Game/Assets/Resources/Scripts/EnemyGrunt.cs
		float distance = distanceToTarget ();
		if(distance < attackRange){
			if (target.CompareTag ("Torch")) {
				target.GetComponent<Torch> ().takeDamage (attackDamage);
			} else if (target.CompareTag ("Player")) {
				target.transform.FindChild ("Torch").GetComponent<Torch> ().takeDamage (attackDamage);
			}
				
			lastAttackTime = Time.time;
		}
=======
		IDamagable damagableObject = gameManager.enemyTarget.GetComponent<IDamagable> ();
		damagableObject.takeDamage (attackDamage);
		//Debug.Log (damagableObject);
		lastAttackTime = Time.time;
>>>>>>> master:Torch/Assets/Resources/Scripts/EnemyGrunt.cs
	}

	//A Coroutine for chasing a target
	private IEnumerator UpdatePath(){

		//First make sure there is a target
		while (target != null && !dead) {
			Vector3 targetPosition = new Vector3 (target.transform.position.x, 0, target.transform.position.z);

			//Set the target position for the Nav Mesh Agent
			navMeshAgent.SetDestination (targetPosition);

			//Make sure that the Nav Mesh Agent refreshes not every frame (to spare costs)
			yield return new WaitForSeconds (refreshTime);
		}
	}


}
