using UnityEngine;
using System.Collections;

public class BomberProjectile : MonoBehaviour {

	public int ObjectPoolerIndex;

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

	void Awake(){
		rb = GetComponent<Rigidbody> ();
	}

	void OnEnable(){
		cam = Camera.main;
		rb.isKinematic = true;
		rb.isKinematic = false;
	}

	public void SetForce(float force){
		forceV3 = transform.forward * force;
	}

	void OnTriggerEnter(Collider other){
        GameObject objectHitted = other.gameObject;
		//Debug.Log(objectHitted.name + " : " + LayerMask.LayerToName(objectHitted.layer));
        if (!objectHitted.CompareTag("EnemyProjectile") && !objectHitted.CompareTag("Enemy") && !objectHitted.CompareTag("Wall Torch"))
        {
            cam.transform.GetComponentInParent<CameraShake>().cameraShake(camShakeLength, camShakeIntensity, camShakeIterationTime);
			GameObject exp = ObjectPooler.Instance.GetObject (16, true, transform.position, Quaternion.Euler (new Vector3 (90, 0, 0)));
			if (exp.transform.position.y < 0) {
				Vector3 pos = exp.transform.position;
				pos.y = 0;
				exp.transform.position = pos;
			}
            exp.GetComponent<explosion>().damage = damage;

            /*IDamagable damagableObject = other.GetComponent<IDamagable> ();
		    GameObject objectHitted = other.gameObject;

		    if (damagableObject != null) {
			    damagableObject.takeDamage (damage);
		    } else if (objectHitted.CompareTag("Player")) {
			    objectHitted.transform.FindChild ("Torch").GetComponent<IDamagable> ().takeDamage (damage);
		    }
            */
			gameObject.SetActive (false);
        }
	}

}
