using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class EndOfRoundCanvas : MonoBehaviour {

	private GameManager gameManager;

	[Serializable]
	public struct PlayerDataText{
		public Text healthPickedUp;
		public Text distanceTravelled;
		public Text shotsFired;
		public Text accuracy;
		public Text scorePickedUp;
		public Text coinsPickedUp;
		public Text powerUpsUsed;
	}
		
	public PlayerDataText[] playerDataTexts;

	public void Fill(){
		if (gameManager == null) {
			gameManager = GameManager.Instance;
		}

		int counter = 0;
		for (int i = 0; i < gameManager.playerManagers.Length; i++) {
			fillPlayerDataTexts (i);
		}
	}

	void fillPlayerDataTexts(int playerManagerIndex){
		PlayerData playerData = gameManager.playerManagers [playerManagerIndex].playerData;
		PlayerDataText dataTexts = playerDataTexts [playerManagerIndex];
		dataTexts.healthPickedUp.text = playerData.healthPickedUp.ToString();
		dataTexts.distanceTravelled.text = playerData.distanceTravelled.ToString();
		dataTexts.shotsFired.text = playerData.shotsFired.ToString();
		playerData.CalculateAccuracy ();
		dataTexts.accuracy.text = playerData.accuracy.ToString () + "%";
		dataTexts.scorePickedUp.text = playerData.scorePickedUp.ToString ();
		dataTexts.coinsPickedUp.text = playerData.coinsPickedUp.ToString ();
		dataTexts.powerUpsUsed.text = playerData.powerUpsUsed.ToString ();
	}
}
