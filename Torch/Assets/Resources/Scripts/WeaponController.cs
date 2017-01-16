using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

	public Weapon startingWeapon;
	public Weapon currentWeapon;
	private PlayerData playerData;

	public Transform weaponHolder;

	protected virtual void Start () {
		//searches for playerdata. Returns null if it is an enemy's weaponcontroller
		playerData = GetComponent<PlayerData> ();
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
		if (playerData != null) {
			currentWeapon.playerData = playerData;
			currentWeapon.damageMultiplier = GameManager.Instance.data.playerDamageMultiplier;
		}
		//Debug.Log ("New weapon equipped: " + currentWeapon);
	}

	public void Fire(){
		currentWeapon.Fire ();
	}
}
