using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerUpInventory : MonoBehaviour {

	[HideInInspector] public int playerNumber;
	private string powerUpButton1, powerUpButton2, powerUpButton3;
	private Image[] powerUpImages;

	public GameObject emptyPowerUpPrefab;
	public int powerUpInventorySize = 3;
	public GameObject[] powerUps;
	private UIInventory uiInventory;

	// Use this for initialization
	void Start () {
		uiInventory = UIInventory.Instance;
		Setup ();
		resetInventory ();
	}

	void Setup(){
		powerUpButton1 = "PowerUpButton1_" + playerNumber;
		powerUpButton2 = "PowerUpButton2_" + playerNumber;
		powerUpButton3 = "PowerUpButton3_" + playerNumber;

		if (playerNumber == 1) {
			powerUpImages = uiInventory.PowerUpImagesP1;
		} else if (playerNumber == 2) {
			powerUpImages = uiInventory.PowerUpImagesP2;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown(powerUpButton1)||(Input.GetAxis (powerUpButton1)<-0.1f))
			UsePowerUp (0);
		if (Input.GetButtonDown(powerUpButton2)||(Input.GetAxis (powerUpButton2)>0.1f))
			UsePowerUp (1);
		if (Input.GetButtonDown(powerUpButton3)||(Input.GetAxis (powerUpButton3)<-0.1f))
			UsePowerUp (2);
	}

	public void AddItemToInventory(GameObject powerUp){
		for (int i = 0; i < powerUps.Length; i++) {
			if (powerUps [i] == emptyPowerUpPrefab) {
				GameObject powerUpEquipped = Instantiate (powerUp, transform) as GameObject;
				powerUps [i] = powerUpEquipped;
				Image icon = UIInventory.Instance.PowerUpImagesP1 [i];
				icon.sprite = powerUp.GetComponent<SpriteRenderer>().sprite;

				Color temp = icon.GetComponent<Image> ().color;
				temp.a = 1f;
				icon.GetComponent<Image> ().color = temp;

				Debug.Log (powerUp + " added to inventory on key " + i);
				return;
			}
		}
	}

	void UsePowerUp(int index){
		if (powerUps [index] != null) {
			powerUps [index].SendMessage ("Use");

			//Select image for Player 1
			Image icon = powerUpImages[index];

			Color temp = icon.GetComponent<Image> ().color;
			temp.a = 0f;
			icon.GetComponent<Image> ().color = temp;

			powerUps [index] = null;

		}
	}

	void resetInventory(){
		powerUps = new GameObject[powerUpInventorySize];
		for (int i = 0; i < powerUpInventorySize; i++) {
			powerUps [i] = emptyPowerUpPrefab;
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
}
