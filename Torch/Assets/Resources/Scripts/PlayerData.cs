using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {

	public int healthPickedUp;
	public float distanceTravelled;
	public int shotsFired;
	public int shotsLanded;
	public float accuracy;
	public int scorePickedUp;
	public int coinsPickedUp;
	public int powerUpsUsed;
	public int enemiesKilled;

	public void resetPlayerData(){
		healthPickedUp = 0;
		distanceTravelled = 0;
		shotsFired = 0;
		shotsLanded = 0;
		accuracy = 0;
		scorePickedUp = 0;
		coinsPickedUp = 0;
		powerUpsUsed = 0;
		enemiesKilled = 0;
	}

	public void CalculateAccuracy(){
		if (shotsFired != 0){
			accuracy = (float)shotsLanded / shotsFired * 100f;
		}
	}

	public void IncrementHealthPickedUp(int amount){
		healthPickedUp += amount;
	}

	public void IncrementDistanceTravelled(int amount){
		distanceTravelled += amount;
	}

	public void IncrementShotsFired(){
		shotsFired += 1;
	}

	public void IncrementShotsLanded(){
		shotsLanded += 1;
	}

	public void IncrementScorePickedUp(int amount){
		scorePickedUp += amount;
	}

	public void IncrementPowerUpsUsed(int amount){
		powerUpsUsed += amount;
	}

	public void GetDistanceTravelled (){
		this.distanceTravelled = GetComponent<PlayerMovement> ().distanceTravelled;
	}

	public void IncrementEnemiesKilled(){
		enemiesKilled += 1;
	}

	public void IncrementCoinsPickedUp(){
		coinsPickedUp += 1;
	}
}
