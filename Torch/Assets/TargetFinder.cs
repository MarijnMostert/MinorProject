using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetFinder : MonoBehaviour {

	public List<GameObject> targets;
	public float radius = 5f;

	void Start(){
		targets = new List<GameObject> ();
	}

	public void FindTargets(){
		targets = new List<GameObject> ();
		Collider[] cols = Physics.OverlapSphere (transform.position, radius);
		foreach (Collider col in cols) {
			if(col.CompareTag("Enemy")){
				targets.Add(col.gameObject);
			}
		}
	}
}
