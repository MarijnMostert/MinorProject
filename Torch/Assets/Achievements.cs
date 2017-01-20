using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Achievements : MonoBehaviour {

	// // // // // // // // // Scripts to steal from
	public GameManager gameManager;
	public Canvas Acanvas;
	public Text Atext;
	public Image Aimage;

	public bool AchievenmentTest;
	public bool[] AchievementList;

	// // // // // // // // // Achievements
	public bool tutorial_done;
	public bool spidernest_destroyed;
	public bool wizarnest_destroyed;
	public bool first_item_bought;
	public bool first_km_walked;
	public bool boss_defeated;

	public bool keys_collected_100;
	public bool all_shopitems_bought;
	public bool home_stairs_climbed;
	public bool died_20_times;
	public bool fell_50_times;
	public bool playtime_1_hour;
	public bool km_42_walked;
	public bool powerups_50_used;
	public bool cheats_unlocked;

	public bool enemies_killed_200;
	public bool spider_100;
	public bool bomber_100;
	public bool minota_100;
	public bool ghost_100;

	public bool levels_10;
	public bool levels_20;
	public bool levels_30;
	public bool levels_40;

	int spidersKilled;
	int minotaurKilled;
	int ghostsKilled;
	int bomberKilled;
	int timesfallen;
	int timesdied;
	int totalcollectedkeys;
	int totalpowerupsused;
	float totaldistance;

	public Vector2  hiddenTarget;
	public Vector2 shownTarget;
	Vector2 target;
	Vector2 damping;

	// // // // // // // // // Reset
	void Start () {
		AchievementList = gameManager.data.achievementsGotten;
		AchievenmentTest = false;					//	0

		tutorial_done = false;			//x doet het!	1
		spidernest_destroyed = false;	//x doet het!	2
		wizarnest_destroyed = false;	//x				3
		first_item_bought = false;					//	4
		first_km_walked = false;					//	5	
		boss_defeated = false;			//x works		6

		keys_collected_100 = false;		//x works		7
		all_shopitems_bought = false;	//x works		8
		home_stairs_climbed = false;				//	9
		died_20_times = false;			//x doet het!	10
		fell_50_times = false;			//x doet het!	11
		playtime_1_hour = false;					//	12
		km_42_walked = false;			//x works		13
		powerups_50_used = false;		//x				14
		cheats_unlocked = false;		//x				15

		enemies_killed_200 = false;		//x works	16
		spider_100 = false;				//x works	17
		bomber_100 = false;				//x works	18
		minota_100 = false;				//x works	19
		ghost_100 = false;				//x works	20

		levels_10 = false;				//x works 	21
		levels_20 = false;				//x works	22
		levels_30 = false;				//x works	23
		levels_40 = false;				//x works	24

		timesfallen = 0;
		timesdied = 0;
		totalcollectedkeys = 0;
		totalpowerupsused = 0;
		totaldistance = 0;

		target = hiddenTarget;

		initializeAllAchievements ();
	}

	void initializeAllAchievements () {
		tutorial_done = AchievementList [1];			//x doet het!	1
		spidernest_destroyed = AchievementList [2];		//x doet het!	2
		wizarnest_destroyed = AchievementList[3];		//x				3
		first_item_bought = AchievementList[4];						//	4
		first_km_walked = AchievementList[5];						//	5	
		boss_defeated = AchievementList[6];				//x works		6

		keys_collected_100 = AchievementList [7];		//x works		7
		all_shopitems_bought = AchievementList [8];		//x works		8
		home_stairs_climbed = AchievementList [9];					//	9
		died_20_times = AchievementList [10];			//x doet het!	10
		fell_50_times = AchievementList [11];			//x doet het!	11
		playtime_1_hour = AchievementList [12];						//	12
		km_42_walked = AchievementList [13];			//x works		13
		powerups_50_used = AchievementList [14];		//x				14
		cheats_unlocked = AchievementList [15];			//x				15

		enemies_killed_200 = AchievementList [16];		//x works	16
		spider_100 = AchievementList [17];				//x works	17
		bomber_100 = AchievementList [18];				//x works	18
		minota_100 = AchievementList [19];				//x works	19
		ghost_100 = AchievementList [20];				//x works	20

		levels_10 = AchievementList [21];				//x works 	21
		levels_20 = AchievementList [22];				//x works	22
		levels_30 = AchievementList [23];				//x works	23
		levels_40 = AchievementList [24];				//x works	24
	}

	void updateAchievement (int i, bool b) {
		AchievementList [i] = b;
		gameManager.data.achievementsGotten [i] = b;
	}

	void Update () {
		if (Input.GetKey ("x")) {
			NewAchievement ("test", 0);
		}
		Aimage.rectTransform.anchoredPosition = Vector2.MoveTowards (Aimage.rectTransform.anchoredPosition, target, 5);
	}

	public void NewAchievement (string text, int i) {
		Debug.Log("New Achievement: " + text);
		updateAchievement (i, true);
		Atext.text = "New Achievement:\n" + text;
		StartCoroutine ("WaitSecs");
	}

	IEnumerator WaitSecs () {
		target = shownTarget;

		yield return new WaitForSeconds (4);

		target = hiddenTarget;
	}

	public void firstBoughtAchievement () {
		if (!first_item_bought) {
			first_item_bought = true;
			NewAchievement ("You've bought your first item!", 4);
		}
	}

	public void walkAchievement(float adding) {
		totaldistance += adding;

		if (!first_km_walked && totaldistance >= 1000) {
			first_km_walked = true;
			NewAchievement ("You've walked your first km!", 5);
		}

		if (!km_42_walked && totaldistance >= 4200) {
			km_42_walked = true;
			NewAchievement ("4200 m walked this play!", 13);
		}
	}

	public void diedAchievement() {
		timesdied++;
		if (!died_20_times && timesdied >= 20) {
			died_20_times = true;
			NewAchievement ("You've died 20 times!", 10);
		}
	}

	public void fallenAchievement () {
		timesfallen++;
		if (!fell_50_times && timesfallen >= 50) {
			fell_50_times = true;
			NewAchievement ("You've fallen 50 times!", 11);
		}
	}

	public void bossAchievement () {
		if (!boss_defeated) {
			boss_defeated = true;
			NewAchievement ("First boss killed!", 6);
		}
	}

	public void nestAchievement(string thisname) {
		switch (thisname) {
		case "SPIDERNEST":
			if (!spidernest_destroyed) {
				spidernest_destroyed = true;
				NewAchievement ("First spidernest destroyed!", 2);
			}
			break;
		case "WIZARDNEST":
			if (!wizarnest_destroyed) {
				wizarnest_destroyed = true;
				NewAchievement ("First wizardnest destroyed!", 3);
			}
			break;
		}
	}

	public void cheatsAchievement () {
		cheats_unlocked = true;
		NewAchievement ("Cheats unlocked!", 15);
	}

	public void powerups50Achievement() {
		totalpowerupsused++;
		if (!powerups_50_used && totalpowerupsused >= 50) {
			powerups_50_used = true;
			NewAchievement ("50 powerups used!", 14);
		}
	}

	public void shopAchievement() {
		all_shopitems_bought = true;
		NewAchievement ("You own all shop items!", 8);
	}

	public void enemiesAchievement() {
		if (!enemies_killed_200) {
			enemies_killed_200 = true;
			NewAchievement ("200 enemies killed!", 16);
		}
	}

	public void tutortialAchievement () {
		if (!tutorial_done) {
			tutorial_done = true;
			NewAchievement ("Tutorial completed!", 1);
		}
	}

	public void collectedKeys () {
		totalcollectedkeys++;
		if (!keys_collected_100 && totalcollectedkeys >= 100) {
			keys_collected_100 = true;
			NewAchievement ("100 keys collected!", 7);
		}
	}

	public void enemiesAchievement (string name) {
		switch (name) {
		case "spider":
			spidersKilled++;
			if (!spider_100 && spidersKilled >= 100) {
				spider_100 = true;
				NewAchievement ("You've killed 100 spiders!", 17);
			}
			break;
		case "minotaur":
			minotaurKilled++;
			if (!minota_100 && minotaurKilled >= 100) {
				minota_100 = true;
				NewAchievement ("You've killed 100 minotaurs!", 19);
			}
			break;
		case "ghost":
			ghostsKilled++;
			if (!ghost_100 && ghostsKilled >= 100) {
				ghost_100 = true;
				NewAchievement ("You've killed 100 ghosts!", 20);
			}
			break;
		case "bomber":
			bomberKilled++;
			if (!bomber_100 && bomberKilled >= 100) {
				bomber_100 = true;
				NewAchievement ("You've killed 100 bombers!", 18);
			}
			break;
		}
	}

	public void levelAchievement (int level) {
		if (level >= 10 && !levels_10) {
			levels_10 = true;
			NewAchievement ("Level 10 reached!", 21);
		}
		if (level >= 20 && !levels_20) {
			levels_20 = true;			
			NewAchievement ("Level 20 reached!", 22);
		}
		if (level >= 30 && !levels_30) {
			levels_30 = true;
			NewAchievement ("Level 30 reached!", 23);
		}
		if (level >= 40 && !levels_30) {
			levels_40 = true;
			NewAchievement ("Level 40 reached!", 24);
		}
	}

}
