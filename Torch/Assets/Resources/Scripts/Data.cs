using UnityEngine;
using System.Collections;

public class Data : MonoBehaviour {

	public bool[] shopItems;
	public int coins;
	public int dungeonLevel;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Tab)) {
			Save ();
		}
	}

	public void Load(){
		for (int i = 0; i < shopItems.Length; i++) {
			string str = "shopItem" + i;
			shopItems [i] = intToBool(PlayerPrefs.GetInt (str));
		}
		coins = PlayerPrefs.GetInt ("coins");
		dungeonLevel = PlayerPrefs.GetInt ("dungeonLevel");

		Debug.Log ("Loaded succesfully");
	}

	public void Save(){
		for (int i = 0; i < shopItems.Length; i++) {
			string str = "shopItem" + i;
			PlayerPrefs.SetInt (str, boolToInt (shopItems [i]));
		}
		PlayerPrefs.SetInt ("coins", coins);
		PlayerPrefs.SetInt ("dungeonLevel", dungeonLevel);

		Debug.Log ("Saved succesfully");
	}

	public void Reset(){
		shopItems = new bool[20];
		for (int i = 0; i < shopItems.Length; i++) {
			string str = "shopItem" + i;
			PlayerPrefs.SetInt (str, 0);
		}
		PlayerPrefs.SetInt ("coins", 0);
		PlayerPrefs.SetInt ("dungeonLevel", 1);

		Debug.Log ("Reset data succesfully");
		Load ();
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
}
