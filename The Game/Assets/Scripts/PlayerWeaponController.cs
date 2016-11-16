using UnityEngine;
using System.Collections;

public class PlayerWeaponController : MonoBehaviour {

	public int playerNumber;
	public GameObject startingWeapon;

	private GameObject currentWeapon;
	private RangedWeapon currentWeaponScript;
	private string fireButton;
	private GameObject weaponHolder;

	// Use this for initialization
	void Start () {
		fireButton = "Fire" + playerNumber;
		weaponHolder = GameObject.Find ("Weapon Holder");
		Equip (startingWeapon);
		currentWeapon = gameObject.transform.FindChild("Weapon Holder").GetChild(0).gameObject;
		currentWeaponScript = currentWeapon.GetComponent<RangedWeapon> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton (fireButton)) {
			currentWeaponScript.fire ();
		}
	}

	//To Equip another weapon
	public void Equip(GameObject weapon){

		//Check if there is already a weapon equipped. If so, destroy it.
		if (currentWeapon != null) {
			Destroy(gameObject.transform.FindChild("Weapon Holder").GetChild(0).gameObject);
		}

		//Instantiate new weapon and equip it.
		GameObject newWeapon = Instantiate (weapon, weaponHolder.transform.position, weaponHolder.transform.rotation, weaponHolder.GetComponent<Transform>()) as GameObject;
		currentWeapon = newWeapon;
		currentWeaponScript = currentWeapon.GetComponent<RangedWeapon> ();
	}
}
