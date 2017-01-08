using UnityEngine;
using System.Collections;

public class RangedWeapon : Weapon {

	public float cooldown;
	public Projectile projectile;
	public float projectileSpeed;
	public AudioClip audioOnFire;

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
		

	//Shooting a projectile
	public override void Fire(){
		if ((Time.time - lastFireTime) > cooldown) {
			Projectile newProjectile = Instantiate (projectile, transform.position, transform.rotation) as Projectile;
			newProjectile.setSpeed (projectileSpeed);
			lastFireTime = Time.time;

			PlayerWeaponController playWeapController = transform.parent.parent.GetComponent<PlayerWeaponController> ();
			if (playWeapController != null) {
				playWeapController.audioSource.clip = audioOnFire;
				playWeapController.audioSource.pitch = Random.Range (.8f, 1.5f);
				playWeapController.audioSource.Play ();
			}
		}

	}
}
