using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectile : MonoBehaviour {

	public int minDamage;
	public int maxDamage;
	public float critChance = 0.05f;
	public float lifeTime;
	public LayerMask collisionMask;
	public float speed;
	public bool piercing = false;
	private List<Enemy> enemiesHit;
	public ParticleSystem particlesOnHit;

	void Start () {
		if (piercing) {
			enemiesHit = new List<Enemy> ();
		}
		//Destroy the bullet after lifeTime seconds
		Destroy (gameObject, lifeTime);
	}
	
	void FixedUpdate () {
		float moveDistance = speed * Time.deltaTime;
		checkCollisions (moveDistance);
		transform.Translate(Vector3.forward * moveDistance);
	}

	public void setSpeed(float newSpeed){
		speed = newSpeed;
	}

	private void checkCollisions(float moveDistance){
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide)) {
			onHitObject (hit);
		}

	}

	private void onHitObject(RaycastHit hit){
		IDamagable damagableObject = hit.collider.GetComponent<IDamagable> ();
		GameObject objectHitted = hit.collider.gameObject;

		if (damagableObject != null) {
			if (piercing && enemiesHit.Contains(objectHitted.GetComponent<Enemy>())){
				return;
			}
			int damage = Random.Range (minDamage, maxDamage);
			bool crit = false;
			if (Random.value < critChance) {
				damage *= 2;
				crit = true;
			}
			
			damagableObject.takeDamage (damage, crit);
			//Debug.Log ("hit " + damagableObject);
		}

		if (objectHitted.CompareTag ("Shield")) {
			return;
		}

		if (particlesOnHit != null) {
			ParticleSystem particles = Instantiate (particlesOnHit, hit.point, Quaternion.identity) as ParticleSystem;
			Destroy (particles.gameObject, particles.duration);
		}
			
		if (piercing && objectHitted.CompareTag ("Enemy")) {
			enemiesHit.Add(objectHitted.GetComponent<Enemy>());
			return;
		}
		
		Destroy (gameObject);
	}
}
