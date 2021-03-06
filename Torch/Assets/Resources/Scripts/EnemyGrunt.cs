﻿using UnityEngine;
using System.Collections;

public class EnemyGrunt : Enemy {

	public int minAttackDamage;
	public int maxAttackDamage;
	public bool attacknow;
    float starttime;
    public bool speedy;

	protected override void Awake(){
		base.Awake ();
	}

	// Use this for initialization
	protected override void OnEnable () {
		if (!firstTimeActive || InstantiatedByObjectPooler) {
			base.OnEnable ();
			StartCoroutine (UpdatePath ());
			//anim = GetComponent<Animator> ();
			attacknow = false;
			if (gameObject.name.Equals ("spider(clone)")) {
				healthBar.transform.localScale.Scale (new Vector3 (3, 3, 3));
			}
			starttime = Time.time;
		} else {
			navMeshAgent.enabled = false;
			firstTimeActive = false;
		}
	}

    // Update is called once per frame
    void Update() {
		if (!dead && (speedy|| Time.time>starttime+1.03)) {
            //if (!agent.enabled) { agent.enabled = true; }
			if (gameManager.enemyTarget != null && distanceToTarget () < attackRange && (Time.time - lastAttackTime) > attackCooldown) {
				StartCoroutine(attack ());
			}
        }
	}

	//If the player is close enough to the torch it will do damage
	IEnumerator attack(){
		if (anim != null) {
//			Debug.Log ("jump");
			anim.SetTrigger ("Attack");
		}
		lastAttackTime = Time.time;
		yield return new WaitForSeconds (.3f);
		ObjectPooler.Instance.PlayAudioSource (clip_attack, mixerGroup, pitchMin, pitchMax, transform);

		IDamagable damagableObject = gameManager.enemyTarget.GetComponent<IDamagable> ();
		if(damagableObject != null){
			damagableObject.takeDamage (Random.Range(minAttackDamage, maxAttackDamage), false, gameObject);
			//Debug.Log (damagableObject);
		}
	}

	//A Coroutine for chasing a target
	private IEnumerator UpdatePath(){
		//First make sure there is a target
		while (gameManager.enemyTarget != null) {
			Vector3 targetPosition = new Vector3 (gameManager.enemyTarget.transform.position.x, 0, gameManager.enemyTarget.transform.position.z);

			//Set the target position for the Nav Mesh Agent
			if (navMeshAgent.enabled) {
				navMeshAgent.SetDestination (targetPosition);
			} else {
				navMeshAgent.enabled = true;
			}

			//Make sure that the Nav Mesh Agent refreshes not every frame (to spare costs)
			yield return new WaitForSeconds (refreshTime);
		}
	}


}
