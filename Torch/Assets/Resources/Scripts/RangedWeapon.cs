using UnityEngine;
using System.Collections;

public class RangedWeapon : Weapon {

	public float cooldown;
	public Projectile projectile;
	public int poolObjectIndex;
	public float projectileSpeed;
	public AudioClip audioOnFire;
	public float pitchShiftMinimal = 0.8f;
	public float pitchShiftMaximal = 1.1f;

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
			Projectile newProjectile = ObjectPooler.Instance.GetObject (projectile.ObjectPoolerIndex, true, transform.position, transform.rotation).GetComponent<Projectile>();
			newProjectile.setSpeed (projectileSpeed);
			lastFireTime = Time.time;

			WeaponController weaponController = transform.parent.parent.GetComponent<WeaponController> ();
			if (weaponController != null) {
				weaponController.audioSource.clip = audioOnFire;
				weaponController.audioSource.pitch = Random.Range (pitchShiftMinimal, pitchShiftMaximal);
				weaponController.audioSource.Play ();
			}
		}

	}
}
