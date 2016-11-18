using UnityEngine;
using System.Collections;

public class BomberProjectile : MonoBehaviour {

	public LayerMask collisionMask;
	public int damage;
	public float lifeTime;

	public Rigidbody rb;
	public Vector3 forceV3;
	public BomberWeapon bomberWeapon;

	void Start () {
		rb = GetComponent<Rigidbody>();
		Destroy (gameObject, lifeTime);
	}

	public void SetForce(float force){
		forceV3 = transform.forward * force;
	}

	void FixedUpdate(){
	}

	void OnTriggerEnter(Collider other){
		IDamagable damagableObject = other.GetComponent<IDamagable> ();
		GameObject objectHitted = other.gameObject;

		if (damagableObject != null) {
			damagableObject.takeDamage (damage);
		} else if (objectHitted.CompareTag("Player")) {
			objectHitted.transform.FindChild ("Torch").GetComponent<IDamagable> ().takeDamage (damage);
		}

		Destroy (this.gameObject);
	}

}
