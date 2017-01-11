using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour {

	public float strength = 1f;
	public float range = 5f;
	public float distanceToKeep = 0f;
	public bool lookAtTarget = false;
	public bool stayActivated = false;

	private bool activated = false;
	private Transform targetObj;
	private Vector3 targetPos;
	private Vector3 smoothDampVar;

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			targetObj = other.transform;
			activated = true;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.CompareTag ("Player") && !stayActivated) {
			activated = false;
		}
	}

	void Update(){
		if (activated && targetObj != null) {
			
			targetPos = (transform.parent.position - targetObj.position).normalized * distanceToKeep + targetObj.position;

			if ((transform.parent.position - targetPos).magnitude < range || stayActivated) {
				transform.parent.position = Vector3.SmoothDamp (transform.parent.position, targetPos, ref smoothDampVar, 1/strength);
				if (lookAtTarget) {
					transform.parent.LookAt (new Vector3 (targetPos.x, transform.parent.position.y, targetPos.z));
				}
			}
		}
	}
}
