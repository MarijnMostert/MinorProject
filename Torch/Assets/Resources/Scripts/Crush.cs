using UnityEngine;
using System.Collections;

public class Crush : MonoBehaviour {

	public float timeBetweenDamage = 1f;
	static float TrapTimer;
	public int damage = 15;

	void Start(){
		TrapTimer = Time.time;
	}

	void OnTriggerStay(Collider other){
		IDamagable damagableObject = other.gameObject.GetComponent<IDamagable> ();
		if (damagableObject != null && Time.time - TrapTimer >= timeBetweenDamage) {
			damagableObject.takeDamage (damage, false);
			Debug.Log (damagableObject + " takes " + damage + " from traps");
			TrapTimer = Time.time;
		}
	}
}
