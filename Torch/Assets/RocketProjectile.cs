using UnityEngine;
using System.Collections;

public class RocketProjectile : MonoBehaviour {

	public Projectile projectileScript;

	void OnTriggerStay(Collider other){
		Debug.Log (other.gameObject);
		if (projectileScript.Target == null && other.CompareTag ("Enemy")) {
			projectileScript.Target = other.gameObject;
		}
	}
}
