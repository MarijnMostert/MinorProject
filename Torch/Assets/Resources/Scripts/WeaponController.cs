using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour {

	public Weapon startingWeapon;
	public Weapon currentWeapon;

	protected Transform weaponHolder;

	protected virtual void Start () {
		weaponHolder = gameObject.transform.FindChild ("Weapon Holder");
		Weapon newWeapon = Instantiate (Weapon, weaponHolder.transform.position, weaponHolder.transform.position, weaponHolder) as Weapon;
		currentWeapon = newWeapon;
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
