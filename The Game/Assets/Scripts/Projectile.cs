using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public LayerMask collisionMask;
	public int damage;
	public float lifeTime;

	private float speed;


	// Use this for initialization
	void Start () {
		Destroy (gameObject, lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
		float moveDistance = speed * Time.deltaTime;
		transform.Translate(Vector3.forward * moveDistance);
		checkCollisions (moveDistance);
	}

	public void setSpeed(float newSpeed){
		speed = newSpeed;
	}

	private void checkCollisions(float moveDistance){
		Ray ray = new Ray (transform.position, Vector3.forward);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide)) {
			onHitObject (hit);
		}

	}

	private void onHitObject(RaycastHit hit){
		IDamagable damagableObject = hit.collider.GetComponent<IDamagable> ();

		if (damagableObject != null) {
			damagableObject.takeDamage (damage);
		}

		Destroy (this.gameObject);
	}
}
