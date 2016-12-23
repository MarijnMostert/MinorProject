using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour {

	public float strength = 1f;
	public float range = 5f;
	public float distanceToKeep = 0f;

	private bool activated = false;
	private Vector3 target;
	private Vector3 smoothDampVar;

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			activated = true;
		}
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			target = (transform.parent.position - other.transform.position).normalized * distanceToKeep + other.transform.position;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			activated = false;
		}
	}

	void Update(){
		if (activated) {
			
			if ((transform.parent.position - target).magnitude < range) {
				transform.parent.position = Vector3.SmoothDamp (transform.parent.position, target, ref smoothDampVar, 1/strength);
			}
		}
	}
}
