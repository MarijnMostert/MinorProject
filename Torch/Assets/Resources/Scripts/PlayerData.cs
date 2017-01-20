using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {

	public Achievements achievements;

	public int healthPickedUp;
	public float distanceTravelled;
	public int shotsFired;
	public int shotsLanded;
	public float accuracy;
	public int scorePickedUp;
	public int coinsPickedUp;
	public int powerUpsUsed;
	public int enemiesKilled;

	void Start () {
		GameObject manager = GameObject.Find ("Game Manager");
		achievements = manager.GetComponent<Achievements> ();
		Debug.Log (achievements);
	}

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
		achievements.walkAchievement (amount);
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
		achievements.powerups50Achievement ();
	}

	public void GetDistanceTravelled (){
		this.distanceTravelled = GetComponent<PlayerMovement> ().distanceTravelled;
	}

	public void IncrementEnemiesKilled(){
		enemiesKilled += 1;

		if (enemiesKilled >= 200) {
			achievements.enemiesAchievement ();
		}
	}

	public void IncrementCoinsPickedUp(){
		coinsPickedUp += 1;
	}
}
