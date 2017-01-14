using UnityEngine;
using System.Collections;
using System;

public class Data : MonoBehaviour {

	[SerializeField] private GameManager gameManager;

	[Header ("- Data")]
	public bool[] shopItemsOwned;
	public bool[] shopItemsEquipped;
	public int coins;
	public int maxAchievedDungeonLevel;
	public bool highQuality;
	public PlayerStats[] playerStats;

	[Serializable]
	public struct PlayerStats{
		public int maxHealth;
		public float damageMultiplier;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Tab)) {
			SaveDataToFile ();
		}
	}

	public void LoadFileToDataAndVars(){
		shopItemsOwned = new bool[20];
		shopItemsEquipped = new bool[20];
		for (int i = 0; i < shopItemsOwned.Length; i++) {
			string strOwned = "shopItem" + i;
			string strEquipped = "shopItemEquipped" + i;
			shopItemsOwned [i] = intToBool(PlayerPrefs.GetInt (strOwned));
			shopItemsEquipped[i] = intToBool(PlayerPrefs.GetInt(strEquipped));
		}
		this.coins = PlayerPrefs.GetInt ("coins");
		this.maxAchievedDungeonLevel = PlayerPrefs.GetInt ("dungeonLevel");
		if (maxAchievedDungeonLevel == 0)
			maxAchievedDungeonLevel = 1;
		this.highQuality = intToBool(PlayerPrefs.GetInt ("highQuality"));

		Debug.Log ("Loaded data succesfully");
	}

	public void SaveDataToFile(){
		for (int i = 0; i < shopItemsOwned.Length; i++) {
			string strOwned = "shopItem" + i;
			PlayerPrefs.SetInt (strOwned, boolToInt (shopItemsOwned [i]));
			string strEquipped = "shopItemEquipped" + i;
			PlayerPrefs.SetInt (strEquipped, boolToInt (shopItemsEquipped [i]));
		}
		PlayerPrefs.SetInt ("coins", coins);
		PlayerPrefs.SetInt ("dungeonLevel", maxAchievedDungeonLevel);
		PlayerPrefs.SetInt ("highQuality", boolToInt(highQuality));

		Debug.Log ("Saved data succesfully");
	}

	public void ResetData(){
		shopItemsOwned = new bool[20];
		shopItemsEquipped = new bool[20];
		for (int i = 0; i < shopItemsOwned.Length; i++) {
			string strOwned = "shopItem" + i;
			PlayerPrefs.SetInt (strOwned, 0);
			string strEquipped = "shopItemEquipped" + i;
			PlayerPrefs.SetInt (strEquipped, 0);
		}
		PlayerPrefs.SetInt ("coins", 0);
		PlayerPrefs.SetInt ("dungeonLevel", 1);
		PlayerPrefs.SetInt ("highQuality", 1);

		Debug.Log ("Reset data succesfully");
		LoadFileToDataAndVars ();
	}

	bool intToBool(int integer){
		if (integer == 0)
			return false;
		else
			return true;
	}

	int boolToInt(bool boolean){
		if(!boolean)
			return 0;
		else
			return 1;
	}

	void OnApplicationQuit(){
		Debug.Log ("Application quitted");
		CollectData ();
		SaveDataToFile ();
	}

	public void ShopItemsToData(){
		for (int i = 0; i < gameManager.shop.itemsToBuy.Length; i++) {
			shopItemsOwned [i] = gameManager.shop.itemsToBuy[i].owned;
			shopItemsEquipped [i] = gameManager.shop.itemsToBuy [i].equipped;
		}
	}

	public void CollectData(){
		ShopItemsToData ();
	}

	public void IncrementCoins(){
		coins += 1;
	}
}
