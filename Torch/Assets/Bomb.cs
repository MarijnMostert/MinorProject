using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

	public float timeTillExplode = 3f;
	public int maxDamage = 120;
	public float blastRange = 3f;
	public ParticleSystem particles;
	public bool canHitPlayer = false;


	// Use this for initialization
	void Start () {
		Invoke ("Explode", timeTillExplode);
	}
	
	void Explode(){
		Collider[] colliders = Physics.OverlapSphere (transform.position, blastRange);
		foreach (Collider collider in colliders) {
			IDamagable damagableObject = collider.GetComponent<IDamagable> ();
			if (damagableObject != null) {
				float distanceFromBomb = Mathf.Abs ((collider.transform.position - transform.position).magnitude);
				int damage = (int)((1 - (distanceFromBomb / blastRange)) * maxDamage);
				if (!canHitPlayer && (collider.gameObject.CompareTag ("Player") || collider.gameObject.CompareTag("Torch"))) {
					damage = 0;
				}
				Debug.Log (damage + " damage on " + collider.gameObject);
				damagableObject.takeDamage (damage, false);
			}
		}
		particles = Instantiate (particles, transform.position, Quaternion.Euler(new Vector3(-90f, 0f, 0f))) as ParticleSystem;
		particles.Play ();
		Destroy (particles.gameObject, particles.duration - 0.2f);
		Destroy (gameObject);
	}
}
