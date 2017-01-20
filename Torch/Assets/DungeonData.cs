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
	public int minLevelBloodWeapon = 16;
	public int minLevelBoomerangWeapon = 20;

	public float chanceIceball = 0.1f;
	public float chancePiercing = 0.1f;
	public float chanceLaser = 0.1f;
	public float chanceBlood = 0.1f;
	public float chanceBoomerang = .1f;

    [Header("- Minimum values traps")]
	public float chanceForTrap = .1f;

    public int minLevelSpidernest = 1;
    public int minLevelWizardnest = 1;
    public int minLevelSpikes = 1;
    public int minLevelWallspikes = 1;
    public int minLevelWallrush = 1;
    public int minLevelShuriken = 1;

    public float chanceSpidernest = 1;
    public float chanceWizardnest = 10;
    public float chanceSpikes = 1;
    public float chanceWallspikes = 1;
    public float chanceWallrush = 1;
    public float chanceShuriken = 1;

	[Header("- Minimum values PuzzleRooms")]
	public GameObject Fliproom;
	public int minLevelFliproom;
	public float chanceFliproom;

	public GameObject Blockpuzzleroom;
	public int minLevelBlockpuzzleroom;
	public float chanceBlockpuzzleroom;

	public GameObject Fallblockpuzzle;
	public int minLevelFallblockpuzzle;
	public float chanceFallblockpuzzle;

	public GameObject Laserroom;
	public int minLevelLaserroom;
	public float chanceLaserroom;

	public GameObject Movingplatformroom;
	public int minLevelMovingplatformroom;
	public float chanceMovingplatformroom;

	public GameObject Bossroom;
	public int minLevelBossroom;
	public float chanceBossroom;

	public GameObject Treasureroom;
	public int minLevelTreasureroom;
	public float chanceTreasureroom;

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
        public Traps Traps;
		public PuzzleRooms puzzleRooms;
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
		public PowerUp bloodWeapon;
		public PowerUp boomerangWeapon;
	}

	[Serializable]
	public struct PowerUp{
		public bool enabled;
		public float spawnChance;
	}

    [Serializable]
    public struct Traps
    {
		public float chanceForTrap;
        public Trap spidernest;
        public Trap wizardnest;
        public Trap spikes;
        public Trap wallspikes;
        public Trap wallrush;
        public Trap shuriken;
    }

    [Serializable]
    public struct Trap
    {
        public bool enabled;
        public float spawnChance;
    }

	[Serializable]
	public struct PuzzleRoom
	{
		public GameObject puzzleRoom;
		public bool enabled;
		public float spawnChance;
	}

	[Serializable]
	public struct PuzzleRooms{
		public PuzzleRoom Fliproom;
		public PuzzleRoom Blockpuzzleroom;
		public PuzzleRoom Fallblockpuzzle;
		public PuzzleRoom Laserroom;
		public PuzzleRoom Movingplatformroom;
		public PuzzleRoom Bossroom;
		public PuzzleRoom Treasureroom;
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
			if (i >= minLevelBloodWeapon)
				DP.powerUps.bloodWeapon.enabled = true;
			if (i >= minLevelBoomerangWeapon)
				DP.powerUps.boomerangWeapon.enabled = true;
            if (i >= minLevelSpidernest)
                DP.Traps.spidernest.enabled = true;
            if (i >= minLevelWizardnest)
                DP.Traps.wizardnest.enabled = true;
            if (i >= minLevelSpikes)
                DP.Traps.spikes.enabled = true;
            if (i >= minLevelWallspikes)
                DP.Traps.wallspikes.enabled = true;
            if (i >= minLevelWallrush)
                DP.Traps.wallrush.enabled = true;
            if (i >= minLevelShuriken)
                DP.Traps.shuriken.enabled = true;
			if (i >= minLevelFliproom)
				DP.puzzleRooms.Fliproom.enabled = true;
			if (i >= minLevelBlockpuzzleroom)
				DP.puzzleRooms.Blockpuzzleroom.enabled = true;
			if (i >= minLevelFallblockpuzzle)
				DP.puzzleRooms.Fallblockpuzzle.enabled = true;
			if (i >= minLevelLaserroom)
				DP.puzzleRooms.Laserroom.enabled = true;
			if (i >= minLevelMovingplatformroom)
				DP.puzzleRooms.Movingplatformroom.enabled = true;
			if (i >= minLevelBossroom)
				DP.puzzleRooms.Bossroom.enabled = true;
			if (i >= minLevelTreasureroom)
				DP.puzzleRooms.Treasureroom.enabled = true;

            DP.powerUps.shield.spawnChance = chanceShield;
			DP.powerUps.sticky.spawnChance = chanceSticky;
			DP.powerUps.wall.spawnChance = chanceWall;
			DP.powerUps.bomb.spawnChance = chanceBomb;
			DP.powerUps.decoy.spawnChance = chanceDecoy;
			DP.powerUps.iceballWeapon.spawnChance = chanceIceball;
			DP.powerUps.piercingWeapon.spawnChance = chancePiercing;
			DP.powerUps.laserWeapon.spawnChance = chanceLaser;
			DP.powerUps.bloodWeapon.spawnChance = chanceBlood;
			DP.powerUps.boomerangWeapon.spawnChance = chanceBoomerang;
			DP.Traps.chanceForTrap = chanceForTrap;
            DP.Traps.spidernest.spawnChance = chanceSpidernest;
            DP.Traps.wizardnest.spawnChance = chanceWizardnest;
            DP.Traps.spikes.spawnChance = chanceSpikes;
            DP.Traps.wallspikes.spawnChance = chanceWallspikes;
            DP.Traps.wallrush.spawnChance = chanceWallrush;
            DP.Traps.shuriken.spawnChance = chanceShuriken;
			DP.puzzleRooms.Blockpuzzleroom.spawnChance = chanceBlockpuzzleroom;
			DP.puzzleRooms.Blockpuzzleroom.puzzleRoom = Blockpuzzleroom;
			DP.puzzleRooms.Fallblockpuzzle.spawnChance = chanceFallblockpuzzle;
			DP.puzzleRooms.Fallblockpuzzle.puzzleRoom = Fallblockpuzzle;
			DP.puzzleRooms.Laserroom.spawnChance = chanceLaserroom;
			DP.puzzleRooms.Laserroom.puzzleRoom = Laserroom;
			DP.puzzleRooms.Bossroom.spawnChance = chanceBossroom;
			DP.puzzleRooms.Bossroom.puzzleRoom = Bossroom;
			DP.puzzleRooms.Movingplatformroom.spawnChance = chanceMovingplatformroom;
			DP.puzzleRooms.Movingplatformroom.puzzleRoom = Movingplatformroom;
			DP.puzzleRooms.Fliproom.spawnChance = chanceFliproom;
			DP.puzzleRooms.Fliproom.puzzleRoom = Fliproom;
			DP.puzzleRooms.Treasureroom.spawnChance = chanceTreasureroom;
			DP.puzzleRooms.Treasureroom.puzzleRoom = Treasureroom;

            DP.timeBetweenSpawns = 15f - spawnRateScaler * i;
			if(DP.timeBetweenSpawns < 4f){
				DP.timeBetweenSpawns = 4f;
			}
			DP.timeBeforeSpawning = 15f - spawnerWarmingUpScaler * i;
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
