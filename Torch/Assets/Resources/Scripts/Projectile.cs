using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public int damage;
	public float lifeTime;
	public LayerMask collisionMask;

	private float speed;

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
	//	GameObject objectHitted = hit.collider.gameObject;

		if (damagableObject != null) {
			damagableObject.takeDamage (damage);
			Debug.Log ("hit " + damagableObject);
		} 

		/*
		else if (objectHitted.CompareTag("Player")) {
			objectHitted.transform.FindChild("Torch Holder").FindChild ("Torch").GetComponent<IDamagable> ().takeDamage (damage);
		}
		*/

		/*
			if (objectHitted.layer.ToString ().Equals ("Player")) {
				GameObject torch;



				if (objectHitted.CompareTag ("Torch")) {
					torch = objectHitted;
				} else {
					torch = objectHitted.transform.FindChild ("Torch").gameObject;
				}
				if (torch != null) {
					torch.GetComponent<IDamagable> ().takeDamage (damage);
				}
			}
		}
		*/

		Destroy (this.gameObject);
	}
}
