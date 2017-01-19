using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class RangedWeapon : Weapon {

	public float cooldown;
	public Projectile projectile;
	public float projectileSpeed;

	private float lastFireTime;

	void Awake(){
		lastFireTime = Time.time;
	}

	//This method is used by the boss to switch its attack
	public void setProjectile(Projectile newProjectile, float cooldown, float projSpeed){
		this.cooldown = cooldown;
		this.projectileSpeed = projSpeed;
		this.projectile = newProjectile;
	}

	public void setCooldown(float cooldown){
		this.cooldown = cooldown;
	}
		

	//Shooting a projectile
	public override void Fire(){
		if ((Time.time - lastFireTime) > cooldown) {
			base.Fire ();
			Projectile newProjectile = ObjectPooler.Instance.GetObject (projectile.ObjectPoolerIndex, true, transform.position, transform.rotation).GetComponent<Projectile>();
			newProjectile.setSpeed (projectileSpeed);
			newProjectile.PlayerData = playerData;
			newProjectile.multiplier = damageMultiplier;
			lastFireTime = Time.time;

			ObjectPooler.Instance.PlayAudioSource (fireClip, mixerGroup, pitchMin, pitchMax, transform);
		}
	}
}
