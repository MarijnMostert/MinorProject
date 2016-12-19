using UnityEngine;
using System.Collections;

public class BomberWeapon : Weapon {

	public float cooldown;
	public BomberProjectile projectile;
	public float force;
	public GameObject weaponHolder;

	private float lastFireTime = 0;

	void Start(){
		weaponHolder = gameObject.transform.parent.gameObject;
	}

	//Shooting a projectile
	public void fire(){
		if ((Time.time - lastFireTime) > cooldown) {
			BomberProjectile obj = Instantiate (projectile, transform.position, Quaternion.identity) as BomberProjectile;
			obj.GetComponent<Rigidbody>().AddForce(force * transform.forward);
			lastFireTime = Time.time;
		}

	}
}
