using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	public Weapon[] weapons;
	public List<Weapon> weaponsList;
	public Weapon emptyWeaponPrefab;
	public int weaponInventorySize = 10;
	public Image indicator;
	private int activeWeapon;
	//public PowerUps[] powerUps;

	void Start () {
		indicator = GameObject.Find ("Inventory Indicator").GetComponent<Image> ();
		weapons = new Weapon[10];

		for (int i = 0; i < weaponInventorySize; i++) {
			weapons [i] = emptyWeaponPrefab;
		}
	}

	void Equip(int index){
		if (weapons [index] != null) {
			gameObject.GetComponent<PlayerWeaponController> ().Equip (weapons [index]);
		}
	}

	public void addToInventory(Weapon weapon){
		for (int i = 0; i < weapons.Length; i++) {
			if (weapons [i] == weapon) {
				return;
			} else if (weapons[i] == emptyWeaponPrefab) {
				weapons [i] = weapon;
				Image icon = GameObject.Find ("WeaponIcon" + i).GetComponent<Image>();
				icon.sprite = weapon.icon;
			//	icon.gameObject.SetActive ();

				//Set alpha color to 1;
				Color temp = icon.GetComponent<Image>().color;
				temp.a = 1f;
				icon.GetComponent<Image>().color = temp;

				indicator.transform.position = icon.transform.position;
				activeWeapon = i;

				Debug.Log (weapon + " added to inventory on key " + i);
				return;
			}
		}
	}
		
	void Update(){
		if (Input.GetKeyDown (KeyCode.Alpha1))
			CheckAndEquip (0);
		if (Input.GetKeyDown (KeyCode.Alpha2))
			CheckAndEquip (1);
		if (Input.GetKeyDown (KeyCode.Alpha3))
			CheckAndEquip (2);
		if (Input.GetKeyDown (KeyCode.Alpha4))
			CheckAndEquip (3);
		if (Input.GetKeyDown (KeyCode.Alpha5))
			CheckAndEquip (4);
		if (Input.GetKeyDown (KeyCode.Alpha6))
			CheckAndEquip (5);
		if (Input.GetKeyDown (KeyCode.Alpha7))
			CheckAndEquip (6);
		if (Input.GetKeyDown (KeyCode.Alpha8))
			CheckAndEquip (7);
		if (Input.GetKeyDown (KeyCode.Alpha9))
			CheckAndEquip (8);
		if (Input.GetKeyDown (KeyCode.Alpha0))
			CheckAndEquip (9);
	}

	private void CheckAndEquip(int index){
		if (weapons [index] != emptyWeaponPrefab && activeWeapon != index) {
			Equip(index);
			indicator.transform.position = GameObject.Find ("WeaponIcon" + index).transform.position;
			activeWeapon = index;
		}
	}

	void SetIndicatorPosition(int index){

	}


}
