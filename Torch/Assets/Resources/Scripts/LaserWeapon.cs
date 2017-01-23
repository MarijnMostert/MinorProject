using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LaserWeapon : Weapon {

	public float cooldown = 0.3f;
	public float laserLength = 200f;
	public float laserTime = 0.05f;
	public LayerMask collisionMask;
	public int minDamage;
	public int maxDamage;
	public float critChance = 0.05f;
	public ParticleSystem particlesOnHit;

	private LineRenderer lineRenderer;
	private Light light_;
	private float lastFireTime;
	private RaycastHit hit;

	void Awake(){
		lastFireTime = Time.time;
		lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.enabled = false;
		light_ = GetComponent<Light> ();
		light_.enabled = false;
	}

	void Start(){
		damageMultiplier = GameManager.Instance.data.playerDamageMultiplier;
	}

	void Update(){
		if (lineRenderer.enabled && Time.time > lastFireTime + laserTime) {
			lineRenderer.enabled = false;
			light_.enabled = false;
		}

	}

	//Shooting a laser
	public override void Fire(){
		if ((Time.time - lastFireTime) > cooldown) {
			ObjectPooler.Instance.PlayAudioSource (fireClip, mixerGroup, pitchMin, pitchMax, transform);
			base.Fire ();
			lineRenderer.enabled = true;
			light_.enabled = true;
			Ray ray = new Ray(transform.position, transform.forward);
			lineRenderer.SetPosition (0, transform.position);
			if (Physics.Raycast (ray, out hit, laserLength, collisionMask)) {
				ObjectPooler.Instance.GetObject(8, true, hit.point,
					Quaternion.Euler(new Vector3(Random.Range(0,360), Random.Range(0,360), Random.Range(0,360))));
				if (hit.collider.gameObject.CompareTag ("Enemy")) {
					bool crit = false;
					int damage = Random.Range ((int)(minDamage*damageMultiplier), (int)(maxDamage*damageMultiplier));
					if (Random.value < critChance) {
						damage *= 2;
						crit = true;
					}
					if (hit.collider.gameObject.CompareTag ("Enemy") || hit.collider.gameObject.CompareTag ("Boss")) {
						hit.collider.gameObject.GetComponent<IDamagable> ().takeDamage (damage, crit, gameObject);
						if (playerData != null) {
							playerData.IncrementShotsLanded ();
						}
					}
				} 
				if (hit.collider.gameObject.CompareTag ("Target")) {
					Debug.Log ("Laser hit target");
					hit.collider.gameObject.GetComponent<RotateTarget> ().Rotate (transform.forward);
				}
				lineRenderer.SetPosition (1, hit.point);
			} else {
				lineRenderer.SetPosition (1, ray.origin + ray.direction * laserLength);
			}
	
			lastFireTime = Time.time;

		}

	}
}
