using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public int damage;
	public float lifeTime;
	public LayerMask collisionMask;
	public float speed;

	void Start () {
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
			damagableObject.takeDamage (damage);
			//Debug.Log ("hit " + damagableObject);
		}
			
		if (!objectHitted.CompareTag ("Shield")) {
			Destroy (this.gameObject);
		}
	}
}
