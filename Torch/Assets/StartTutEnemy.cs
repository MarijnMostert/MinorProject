using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class StartTutEnemy : MonoBehaviour {

	public GameObject spider;
	NavMeshAgent spiderAgent;

	// Use this for initialization
	void Start () {
		spiderAgent = spider.GetComponent<NavMeshAgent> ();
		spiderAgent.speed = 0;
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			spiderAgent.speed = 2;
		}
	}
}