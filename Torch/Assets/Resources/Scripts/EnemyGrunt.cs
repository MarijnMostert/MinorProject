﻿using UnityEngine;
using System.Collections;

public class EnemyGrunt : Enemy {

	protected override void Awake(){
		base.Awake ();
	}

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		StartCoroutine (UpdatePath ());

	}
	
	// Update is called once per frame
	void Update () {
		if (gameManager.enemyTarget != null && distanceToTarget () < attackRange && (Time.time - lastAttackTime) > attackCooldown) {
			attack ();
		}
	}

	//If the player is close enough to the torch it will do damage
	private void attack(){
		IDamagable damagableObject = gameManager.enemyTarget.GetComponent<IDamagable> ();
		damagableObject.takeDamage (attackDamage);
		//Debug.Log (damagableObject);
		lastAttackTime = Time.time;
	}

	//A Coroutine for chasing a target
	private IEnumerator UpdatePath(){
		//First make sure there is a target
		while (gameManager.enemyTarget != null) {
			Vector3 targetPosition = new Vector3 (gameManager.enemyTarget.transform.position.x, -1.25f, gameManager.enemyTarget.transform.position.z);

			//Set the target position for the Nav Mesh Agent
			navMeshAgent.SetDestination (targetPosition);

			//Make sure that the Nav Mesh Agent refreshes not every frame (to spare costs)
			yield return new WaitForSeconds (refreshTime);
		}
	}


}
