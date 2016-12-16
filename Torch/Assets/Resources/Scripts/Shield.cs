using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

	private GameManager gameManager;

	void Start(){
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
		gameManager.torch.isDamagable = false;
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("EnemyProjectile")){
			Projectile projectile = other.gameObject.GetComponent<Projectile>();
			projectile.setSpeed(-1 * projectile.speed);
		}
	}

	void OnDestroy(){
		gameManager.torch.isDamagable = true;
	}
}
