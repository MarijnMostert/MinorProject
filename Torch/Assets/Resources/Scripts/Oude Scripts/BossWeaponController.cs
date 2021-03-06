using UnityEngine;
using System.Collections;

public class BossWeaponController : MonoBehaviour {

	public Weapon normalWeapon;
	public Weapon specialWeapon;

	protected Transform weaponHolder;

	protected virtual void Start () {
		weaponHolder = gameObject.transform.FindChild ("Weapon Holder");
		Equip (normalWeapon);
		Equip (specialWeapon);
	}

	//To Equip another weapon
	public virtual void Equip(Weapon weapon){
		//Instantiate new weapon and equip it.
		Weapon newWeapon = Instantiate (weapon, weaponHolder.transform.position, weaponHolder.transform.rotation, weaponHolder) as Weapon;
	}

	public void Fire(){
		normalWeapon.Fire ();
	}

	public void SpecialAttack(){
		specialWeapon.Fire ();
	}
}
