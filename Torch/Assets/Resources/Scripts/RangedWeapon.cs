using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RangedWeapon : Weapon {

	public float cooldown;
	public Projectile projectile;
	public float projectileSpeed;

	private float lastFireTime;

	void Awake(){
		lastFireTime = Time.time;
	}

	//Shooting a projectile
	public override void Fire(){
		if ((Time.time - lastFireTime) > cooldown) {
			Projectile newProjectile = Instantiate (projectile, transform.position, transform.rotation) as Projectile;
			newProjectile.setSpeed (projectileSpeed);
			lastFireTime = Time.time;
		}

	}
}
