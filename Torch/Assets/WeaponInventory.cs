using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponInventory : MonoBehaviour {

	private GameManager gameManager;
	private UIInventory uiInventory;
	private PlayerWeaponController[] playerWeaponControllers;
	public Weapon[] weapons;
	public Weapon emptyWeaponPrefab;
	public int weaponInventorySize = 6;
	[SerializeField] private int[] activeWeapon;
	public Image[] indicator;
	public Vector3[] indicatorOffset;

	void Start () {
		gameManager = GameManager.Instance;

		uiInventory = GetComponent<UIInventory> ();

		for(int i = 0; i<2; i++){
			indicator [i].transform.position = uiInventory.WeaponImages [0].transform.position;
			if (gameManager.numberOfPlayers == 2) {
				indicator [i].transform.position += indicatorOffset [i];
			}
		}

		playerWeaponControllers = new PlayerWeaponController[2];
		for(int i = 0; i<2; i++){
			playerWeaponControllers [i] = gameManager.playerManagers [i].playerWeaponController;
		}

		weapons = new Weapon[weaponInventorySize];
		for (int i = 0; i < weaponInventorySize; i++) {
			weapons [i] = emptyWeaponPrefab;
		}
	}

	void ResetInventory(){
		weapons = new Weapon[weaponInventorySize];
		for (int i = 0; i < weaponInventorySize; i++) {
			weapons [i] = emptyWeaponPrefab;
		}
		activeWeapon = new int[2];
	}

	void Update(){
		if (Input.GetButtonDown ("NextWeapon1"))
			NextWeapon (1);
		if (Input.GetButtonDown ("PrevWeapon1"))
			PrevWeapon (1);
		if (Input.GetButtonDown ("NextWeapon2"))
			NextWeapon (2);
		if (Input.GetButtonDown ("PrevWeapon2"))
			PrevWeapon (2);
		if (Input.GetKeyDown (KeyCode.Alpha1))
			CheckAndEquip (0, 1);
		if (Input.GetKeyDown (KeyCode.Alpha2))
			CheckAndEquip (1, 1);
		if (Input.GetKeyDown (KeyCode.Alpha3))
			CheckAndEquip (2, 1);
		if (Input.GetKeyDown (KeyCode.Alpha4))
			CheckAndEquip (3, 1);
		if (Input.GetKeyDown (KeyCode.Alpha5))
			CheckAndEquip (4, 1);
		if (Input.GetKeyDown (KeyCode.Alpha6))
			CheckAndEquip (5, 1);
		/*if (Input.GetKeyDown (KeyCode.Alpha7))
			CheckAndEquip (6, 1);
		if (Input.GetKeyDown (KeyCode.Alpha8))
			CheckAndEquip (7, 1);
		if (Input.GetKeyDown (KeyCode.Alpha9))
			CheckAndEquip (8, 1);
		if (Input.GetKeyDown (KeyCode.Alpha0))
			CheckAndEquip (9, 1);
			*/
	}

	public bool AddWeaponToInventory(Weapon weapon, int playerNumber){
		for (int i = 0; i < weapons.Length; i++) {
			if (weapons [i] == weapon) {
				return false;
			} else if (weapons[i] == emptyWeaponPrefab) {
				weapons [i] = weapon;
				Image icon = uiInventory.WeaponImages [i];
				icon.sprite = weapon.icon;

				//Set alpha color to 1;
				Color temp = icon.GetComponent<Image>().color;
				temp.a = 1f;
				icon.GetComponent<Image>().color = temp;

				indicator [playerNumber - 1].transform.position = icon.transform.position;
				if (gameManager.numberOfPlayers == 2) {
					indicator [playerNumber - 1].transform.position += indicatorOffset [playerNumber - 1];
				}
				activeWeapon[playerNumber - 1] = i;

				Debug.Log (weapon + " added to inventory on key " + i);
				return true;
			}
		}
		return false;
	}

	void Equip(int index, int playerNumber){
		if (weapons [index] != null) {
			playerWeaponControllers [playerNumber - 1].Equip (weapons [index]);
		}
	}

	void NextWeapon(int playerNumber){
		if (activeWeapon[playerNumber - 1] != (weapons.Length - 1)) {
			CheckAndEquip (activeWeapon[playerNumber - 1] + 1, playerNumber);
		} else {
			CheckAndEquip (0, playerNumber);
		}
	}

	void PrevWeapon(int playerNumber){
		if (activeWeapon[playerNumber - 1] != 0) {
			CheckAndEquip (activeWeapon[playerNumber - 1] - 1, playerNumber);
		} else {
			CheckAndEquip (weapons.Length - 1, playerNumber);
		}
	}

	void CheckAndEquip(int index, int playerNumber){
		if (weapons [index] != emptyWeaponPrefab && activeWeapon[playerNumber - 1] != index) {
			Equip(index, playerNumber);
			indicator [playerNumber - 1].transform.position = uiInventory.WeaponImages [index].transform.position;
			if (gameManager.numberOfPlayers == 2) {
				indicator [playerNumber - 1].transform.position += indicatorOffset [playerNumber - 1];
			}
			activeWeapon[playerNumber - 1] = index;
		}
	}

	public void ApplyIndicatorOffset(){
		for (int i = 0; i < 2; i++) {
			indicator [i].transform.position = GetComponent<UIInventory>().WeaponImages[activeWeapon[i]].transform.position + indicatorOffset [i];
		}
	}

	public void RemoveIndicatorOffset(){
		for (int i = 0; i < 2; i++) {
			if (indicator [i] != null) {
				indicator [i].transform.position = GetComponent<UIInventory> ().WeaponImages [activeWeapon[i]].transform.position;
			}
		}
	}
}
