using UnityEngine;
using System.Collections;

public class BossBulletVision : MonoBehaviour {

	private GameObject parent;

	void Start(){
		parent = transform.parent.gameObject;
	}
		
	//Sense bullets
	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag("Bullet")) {
			Debug.Log ("Bullet Enter");
			parent.GetComponent<Boss>().bulletsNearby.Add (other.gameObject);
		}
	}

	//sense bullets
	void OnTriggerStay(Collider other){
		if(other.gameObject.CompareTag("Bullet") && !parent.GetComponent<Boss>().bulletsNearby.Contains(other.gameObject)){
			Debug.Log ("Bullet stay");
			parent.GetComponent<Boss>().bulletsNearby.Add (other.gameObject);
		}
	}

	//Remove old bullets
	void OnTriggerExit(Collider other){
		if (other.gameObject.CompareTag("Bullet")) {
			Debug.Log ("Bullet exit");
			parent.GetComponent<Boss>().bulletsNearby.Remove (other.gameObject);
		}
	}
}
