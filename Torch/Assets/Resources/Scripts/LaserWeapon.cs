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

	private LineRenderer lineRenderer;
	private Light light;
	private float lastFireTime;
	private RaycastHit hit;

	void Awake(){
		lastFireTime = Time.time;
		lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.enabled = false;
		light = GetComponent<Light> ();
		light.enabled = false;
	}

	void Update(){
		if (lineRenderer.enabled && Time.time > lastFireTime + laserTime) {
			lineRenderer.enabled = false;
			light.enabled = false;
		}

	}

	//Shooting a laser
	public override void Fire(){
		if ((Time.time - lastFireTime) > cooldown) {
			lineRenderer.enabled = true;
			light.enabled = true;
			Ray ray = new Ray(transform.position, transform.forward);
			lineRenderer.SetPosition (0, transform.position);
			if (Physics.Raycast (ray, out hit, laserLength, collisionMask)) {
				if (hit.collider.gameObject.CompareTag ("Enemy")) {
					bool crit = false;
					int damage = Random.Range (minDamage, maxDamage);
					if (Random.value < critChance) {
						damage *= 2;
						crit = true;
					}
					hit.collider.gameObject.GetComponent<Enemy> ().takeDamage (damage, crit);
				} 
				lineRenderer.SetPosition (1, hit.point);
			} else {
				lineRenderer.SetPosition (1, ray.origin + ray.direction * laserLength);
			}
	
			lastFireTime = Time.time;
		}

	}
}
