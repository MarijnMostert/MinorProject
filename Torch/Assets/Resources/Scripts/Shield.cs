using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("EnemyProjectile")){
			Projectile projectile = other.gameObject.GetComponent<Projectile>();
			projectile.setSpeed(-1 * projectile.speed);
		}
	}
}
