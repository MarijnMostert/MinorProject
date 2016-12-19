using UnityEngine;
using System.Collections;

public class BomberProjectile : MonoBehaviour {

	public LayerMask collisionMask;
	public int damage;
	public float lifeTime;

	public Rigidbody rb;
	public Vector3 forceV3;
	public BomberWeapon bomberWeapon;

	public float camShakeLength;
	public float camShakeIntensity;
	public float camShakeIterationTime;

	private Camera cam;
    public GameObject explosion;

	void Start () {
		rb = GetComponent<Rigidbody>();		
		cam = Camera.main;
		Destroy (gameObject, lifeTime);
	}

	public void SetForce(float force){
		forceV3 = transform.forward * force;
	}

	void OnTriggerEnter(Collider other){
        GameObject objectHitted = other.gameObject;
        if (!objectHitted.CompareTag("EnemyProjectile") && !objectHitted.CompareTag("Enemy"))
        {
            cam.transform.GetComponentInParent<CameraShake>().cameraShake(camShakeLength, camShakeIntensity, camShakeIterationTime);
            GameObject exp = Instantiate(explosion, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.Euler(new Vector3(90, 0, 0))) as GameObject;
            exp.GetComponent<explosion>().damage = damage;

            /*IDamagable damagableObject = other.GetComponent<IDamagable> ();
		    GameObject objectHitted = other.gameObject;

		    if (damagableObject != null) {
			    damagableObject.takeDamage (damage);
		    } else if (objectHitted.CompareTag("Player")) {
			    objectHitted.transform.FindChild ("Torch").GetComponent<IDamagable> ().takeDamage (damage);
		    }
            */
            Destroy(this.gameObject);
        }
	}

}
