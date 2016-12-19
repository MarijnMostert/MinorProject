using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	public Weapon[] weapons;
	public List<Weapon> weaponsList;
	public Weapon emptyWeaponPrefab;
	public GameObject emptyPowerUpPrefab;
	public int weaponInventorySize = 10;
	public int powerUpInventorySize = 3;
	public Image indicator;
	private int activeWeapon;
	public GameObject[] powerUps;

	void Start () {
		indicator = GameObject.Find ("Inventory Indicator").GetComponent<Image> ();
		resetInventory ();
	}

	void Equip(int index){
		if (weapons [index] != null) {
			gameObject.GetComponent<PlayerWeaponController> ().Equip (weapons [index]);
		}
	}

	public bool AddWeaponToInventory(Weapon weapon){
		for (int i = 0; i < weapons.Length; i++) {
			if (weapons [i] == weapon) {
				return false;
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
				return true;
			}
		}
		return false;
	}

	public void AddItemToInventory(GameObject powerUp, GameObject player){
		for (int i = 0; i < powerUps.Length; i++) {
			if (powerUps [i] == emptyPowerUpPrefab) {
				GameObject powerUpEquipped = Instantiate (powerUp, player.transform) as GameObject;
				powerUps [i] = powerUpEquipped;
				Image icon = GameObject.Find ("PowerUpIcon" + i).GetComponent<Image> ();
				icon.sprite = powerUp.GetComponent<SpriteRenderer>().sprite;

				Color temp = icon.GetComponent<Image> ().color;
				temp.a = 1f;
				icon.GetComponent<Image> ().color = temp;

				Debug.Log (powerUp + " added to inventory on key " + i);
				return;
			}
		}
	}

	void Update(){
		if (Input.GetButtonDown ("NextWeapon1"))
			NextWeapon ();
		if (Input.GetButtonDown ("PrevWeapon1"))
			PrevWeapon ();
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
		if (Input.GetButtonDown("PowerUpButton1_1"))
			UsePowerUp (0);
		if (Input.GetButtonDown("PowerUpButton2_1"))
			UsePowerUp (1);
		if (Input.GetButtonDown("PowerUpButton3_1"))
			UsePowerUp (2);
	}

	private void CheckAndEquip(int index){
		if (weapons [index] != emptyWeaponPrefab && activeWeapon != index) {
			Equip(index);
			indicator.transform.position = GameObject.Find ("WeaponIcon" + index).transform.position;
			activeWeapon = index;
		}
	}

	void NextWeapon(){
		if (activeWeapon != 9) {
			CheckAndEquip (activeWeapon + 1);
		}
	}

	void PrevWeapon(){
		if(activeWeapon != 0) {
			CheckAndEquip (activeWeapon - 1);
		}
	}

	void UsePowerUp(int index){
		if (powerUps [index] != null) {
			powerUps [index].SendMessage ("Use");


			Image icon = GameObject.Find ("PowerUpIcon" + index).GetComponent<Image> ();
			Color temp = icon.GetComponent<Image> ().color;
			temp.a = 0f;
			icon.GetComponent<Image> ().color = temp;

			powerUps [index] = null;

		}
	}

	public bool isFull(){
		for (int i = 0; i < powerUps.Length; i++) {
			if (powerUps [i] == emptyPowerUpPrefab || powerUps[i] == null) {
				return false;
			}
		}
		return true;
	}

	public void resetInventory(){
		weapons = new Weapon[weaponInventorySize];
		for (int i = 0; i < weaponInventorySize; i++) {
			weapons [i] = emptyWeaponPrefab;
		}

		powerUps = new GameObject[powerUpInventorySize];
		for (int i = 0; i < powerUpInventorySize; i++) {
			powerUps [i] = emptyPowerUpPrefab;
		}
	}

}
