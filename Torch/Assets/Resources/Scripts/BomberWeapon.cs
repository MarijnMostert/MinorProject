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
			BomberProjectile obj = ObjectPooler.Instance.GetObject (projectile.ObjectPoolerIndex, true, transform.position, Quaternion.identity).GetComponent<BomberProjectile>();
			obj.GetComponent<Rigidbody>().AddForce(force * transform.forward);
			ObjectPooler.Instance.PlayAudioSource (fireClip, mixerGroup, pitchMin, pitchMax, transform);
			lastFireTime = Time.time;
		}

	}
}
