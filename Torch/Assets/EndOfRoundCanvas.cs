using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class EndOfRoundCanvas : MonoBehaviour {

	private GameManager gameManager;

	public Text timeUsed;
	[Serializable]
	public struct PlayerDataText{
		public Text healthPickedUp;
		public Text distanceTravelled;
		public Text shotsFired;
		public Text accuracy;
		public Text scorePickedUp;
		public Text coinsPickedUp;
		public Text powerUpsUsed;
		public Text enemiesKilled;
	}
		
	public PlayerDataText[] playerDataTexts;

	public void Fill(){
		if (gameManager == null) {
			gameManager = GameManager.Instance;
		}

		for (int i = 0; i < gameManager.playerManagers.Length; i++) {
			fillPlayerDataTexts (i);
		}
	}

	void fillPlayerDataTexts(int playerManagerIndex){
		PlayerData playerData = gameManager.playerManagers [playerManagerIndex].playerData;
		PlayerDataText dataTexts = playerDataTexts [playerManagerIndex];
		dataTexts.healthPickedUp.text = playerData.healthPickedUp.ToString();
		playerData.GetDistanceTravelled ();
		dataTexts.distanceTravelled.text = DistanceToString(playerData.distanceTravelled);
		dataTexts.shotsFired.text = playerData.shotsFired.ToString();
		playerData.CalculateAccuracy ();
		dataTexts.accuracy.text = ((int)playerData.accuracy).ToString () + "%";
		dataTexts.scorePickedUp.text = playerData.scorePickedUp.ToString ();
		dataTexts.coinsPickedUp.text = playerData.coinsPickedUp.ToString ();
		dataTexts.powerUpsUsed.text = playerData.powerUpsUsed.ToString ();
		dataTexts.enemiesKilled.text = playerData.enemiesKilled.ToString ();
		timeUsed.text = GameManager.Instance.ui.timer.timerText.text;
	}

	public string DistanceToString(float distanceTravelled){
		if (distanceTravelled < 1000) {
			return (int)distanceTravelled + " m";
		} else {
			string km = (distanceTravelled / 1000).ToString ("##.0") + " km";
			return km;
		}
	}

}
