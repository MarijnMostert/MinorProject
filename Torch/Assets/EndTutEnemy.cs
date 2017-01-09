using UnityEngine;
using System.Collections;

public class EndTutEnemy : MonoBehaviour {

	public GameObject spider;
	EnemyGrunt spiderScript;
	Doors thisdoor;

	// Use this for initialization
	void Start () {
		spiderScript = spider.GetComponent<EnemyGrunt> ();
		thisdoor = GetComponent<Doors> ();
	}

	void OnTriggerEnter () {
		if (spiderScript.getHealth () <= 0) {
			thisdoor.locked = false;
			thisdoor.Open ();
		}
	}
}
