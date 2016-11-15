using UnityEngine;
using System.Collections;

public class RangedWeapon : MonoBehaviour {

	public float cooldown;
	public Projectile projectile;
	public float projectileSpeed;

	private float lastFireTime;


	void Start () {
		lastFireTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void fire(){
		if ((Time.time - lastFireTime) > cooldown) {
			Projectile newProjectile = Instantiate (projectile, transform.position, transform.rotation) as Projectile;
			newProjectile.setSpeed (projectileSpeed);
			lastFireTime = Time.time;
		}

	}
}
