using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public LayerMask collisionMask;
	public int damage;
	public float lifeTime;

	public IDamagable damagableObject;
	public GameObject objectHitted;

	private float speed;


	// Use this for initialization
	void Start () {
		Destroy (gameObject, lifeTime);
	}
	
	// Update is called once per frame
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
			Debug.Log (hit);
			onHitObject (hit);
		}

	}

	private void onHitObject(RaycastHit hit){
		damagableObject = hit.collider.GetComponent<IDamagable> ();
		objectHitted = hit.collider.gameObject;

		if (damagableObject != null) {
			damagableObject.takeDamage (damage);
		} else if (objectHitted.CompareTag("Player")) {
			objectHitted.transform.FindChild ("Torch").GetComponent<IDamagable> ().takeDamage (damage);
		}

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
		if (objectHitted != GameObject.Find ("BulletSenser")) {
			Destroy (this.gameObject);
		}
	}
}
