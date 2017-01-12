using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

	public Weapon startingWeapon;
	public Weapon currentWeapon;

	public Transform weaponHolder;

	protected virtual void Start () {
		Equip (startingWeapon);
	}

	//To Equip another weapon
	public virtual void Equip(Weapon weapon){

		//Check if there is already a weapon equipped. If so, destroy it.
		if (currentWeapon != null) {
			Destroy(currentWeapon.gameObject);
		}

		//Instantiate new weapon and equip it.
		Weapon newWeapon = Instantiate (weapon, weaponHolder.transform.position, weaponHolder.transform.rotation, weaponHolder) as Weapon;
		currentWeapon = newWeapon;
		//Debug.Log ("New weapon equipped: " + currentWeapon);
	}

	public void Fire(){
		currentWeapon.Fire ();
	}
}
