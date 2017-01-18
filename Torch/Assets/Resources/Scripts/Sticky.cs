using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sticky : AudioObject {
	
	public AudioClip Clip;
	public float stickyTime;
	private List<GameObject> enemies;

	void Start(){
		enemies = new List<GameObject> ();
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Enemy")) {
			StartCoroutine (MyCoroutine(other));
		}
	}

	IEnumerator MyCoroutine(Collider other){
		ObjectPooler.Instance.PlayAudioSource (Clip, mixerGroup, pitchMin, pitchMax, transform);
		GameObject enemy = other.gameObject;
		enemies.Add (enemy);
		NavMeshAgent agent = enemy.GetComponent <NavMeshAgent>();
		float enemySpeed = agent.speed;
		agent.speed = 0;
		yield return new WaitForSeconds (stickyTime);
		if (agent != null) {
			agent.speed = enemySpeed;
		}
		ReleaseAndDestroy ();
	}

	void ReleaseAndDestroy(){
		for(int i = 0; i < enemies.Count; i++) {
			if (enemies [i] != null) {
				enemies [i].GetComponent<NavMeshAgent> ().speed = enemies [i].GetComponent<Enemy> ().speed;
			}
		}
		Destroy (gameObject);
	}

}
