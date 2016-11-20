using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

	public Weapon startingWeapon;
	public Weapon currentWeapon;

	protected GameObject weaponHolder;

	void Awake(){
		weaponHolder = gameObject.transform.FindChild ("Weapon Holder").gameObject;
		Equip (startingWeapon);
	}

	protected virtual void Start () {
		weaponHolder = gameObject.transform.FindChild ("Weapon Holder").gameObject;
		Equip (startingWeapon);
	}

	//To Equip another weapon
	public void Equip(Weapon weapon){

		//Check if there is already a weapon equipped. If so, destroy it.
		if (currentWeapon != null) {
			Destroy(currentWeapon.gameObject);
		}

		//Instantiate new weapon and equip it.
		Weapon newWeapon = Instantiate (weapon, weaponHolder.transform.position, weaponHolder.transform.rotation, weaponHolder.GetComponent<Transform>()) as Weapon;
		currentWeapon = newWeapon;
	}
}
