using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DungeonData : MonoBehaviour {

	[Header ("- General")]
	public int amountOfLevels = 40;
	public int chanceOfRoom = 5; //Dit is de 1/n kans op een kamer, dus groter getal is kleinere kans
	public float spawnRateScaler = .2f;
	public float spawnerWarmingUpScaler = .3f;

	[Header("- Minimum spawn level enemies")]
	public int minLevelForMinotaur = 1;
	public int minLevelForSpider = 3;
	public int minLevelForGhost = 6;
	public int minLevelForBomber = 9;

	[Header("- Minimum spawn level powerUps")]
	public int minLevelForShield = 0;
	public int minLevelForSticky = 2;
	public int minLevelForWall = 5;
	public int minLevelForBomb = 8;
	public int minLevelForDecoy = 11;

	public float chanceShield = .2f;
	public float chanceSticky = .2f;
	public float chanceWall = .2f;
	public float chanceBomb = .2f;
	public float chanceDecoy = .2f;

	[Header("- Minimum spawn level Weapons")]
	public int minLevelIceballWeapon = 2;
	public int minLevelPiercingWeapon = 7;
	public int minLevelLaserWeapon = 13;

	public float chanceIceball = 0.1f;
	public float chancePiercing = 0.1f;
	public float chanceLaser = 0.1f;

	public DungeonParameters[] dungeonParameters;

	[Serializable]
	public struct DungeonParameters{
		public int width;
		public int height;
		public int minAmountOfRooms;
		public int maxAmountOfRooms;
		public int chanceOfRoom;
		public float timeBetweenSpawns;
		public float timeBeforeSpawning;
		public bool spiderNests;
		public Enemies enemies;
		public PowerUps powerUps;
	}

	[Serializable]
	public struct Enemies{
		public bool minoTaur;
		public bool Spider;
		public bool Ghost;
		public bool Bomber;
	}

	[Serializable]
	public struct PowerUps{
		public PowerUp shield;
		public PowerUp sticky;
		public PowerUp wall;
		public PowerUp decoy;
		public PowerUp bomb;
		public PowerUp iceballWeapon;
		public PowerUp piercingWeapon;
		public PowerUp laserWeapon;
	}

	[Serializable]
	public struct PowerUp{
		public bool enabled;
		public float spawnChance;
	}

	void Setup(){
		dungeonParameters = new DungeonParameters[amountOfLevels];
		for (int i = 0; i < amountOfLevels; i++) {
			DungeonParameters DP = new DungeonParameters();
			DP.width = 20 + (i * 2);
			DP.height = 20 + (i * 2);
			DP.minAmountOfRooms = 2 + (int)(i * .5);
			DP.maxAmountOfRooms = 3 + (int)(i * .8);
			DP.chanceOfRoom = chanceOfRoom;

			if(i>= minLevelForMinotaur)
				DP.enemies.minoTaur = true;
			if (i >= minLevelForSpider)
				DP.enemies.Spider = true;
			if (i >= minLevelForGhost)
				DP.enemies.Ghost = true;
			if (i >= minLevelForBomber)
				DP.enemies.Bomber = true;
			if (i >= minLevelForShield)
				DP.powerUps.shield.enabled = true;
			if (i >= minLevelForSticky)
				DP.powerUps.sticky.enabled = true;
			if (i >= minLevelForWall)
				DP.powerUps.wall.enabled = true;
			if (i >= minLevelForBomb)
				DP.powerUps.bomb.enabled = true;
			if (i >= minLevelForDecoy)
				DP.powerUps.decoy.enabled = true;
			if (i >= minLevelIceballWeapon)
				DP.powerUps.iceballWeapon.enabled = true;
			if (i >= minLevelPiercingWeapon)
				DP.powerUps.piercingWeapon.enabled = true;
			if (i >= minLevelLaserWeapon)
				DP.powerUps.laserWeapon.enabled = true;

			DP.powerUps.shield.spawnChance = chanceShield;
			DP.powerUps.sticky.spawnChance = chanceSticky;
			DP.powerUps.wall.spawnChance = chanceWall;
			DP.powerUps.bomb.spawnChance = chanceBomb;
			DP.powerUps.decoy.spawnChance = chanceDecoy;
			DP.powerUps.iceballWeapon.spawnChance = chanceIceball;
			DP.powerUps.piercingWeapon.spawnChance = chancePiercing;
			DP.powerUps.laserWeapon.spawnChance = chanceLaser;

			DP.timeBetweenSpawns = 15f - spawnRateScaler * i;
			if(DP.timeBetweenSpawns < 4f){
				DP.timeBetweenSpawns = 4f;
			}
			DP.timeBeforeSpawning = 15f - spawnRateScaler * i;
			if (DP.timeBeforeSpawning < 2f) {
				DP.timeBeforeSpawning = 2f;
			}


			dungeonParameters [i] = DP;
		}
	}

	void Start(){
		Setup ();
	}


}
