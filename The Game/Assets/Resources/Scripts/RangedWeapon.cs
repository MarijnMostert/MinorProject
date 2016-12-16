using UnityEngine;
using System.Collections;

public class RangedWeapon : Weapon {

	public float cooldown;
	public Projectile projectile;
	public float projectileSpeed;

	private float lastFireTime = 0;

	//This method is used by the boss to switch its attack
	public void setProjectile(Projectile newProjectile, float cooldown, float projSpeed){
		this.cooldown = cooldown;
		this.projectileSpeed = projSpeed;
		this.projectile = newProjectile;
	}
		

	//Shooting a projectile
	public void fire(){
		if ((Time.time - lastFireTime) > cooldown) {
			Projectile newProjectile = Instantiate (projectile, transform.position, transform.rotation) as Projectile;
			newProjectile.setSpeed (projectileSpeed);
			lastFireTime = Time.time;
		}

	}
}
