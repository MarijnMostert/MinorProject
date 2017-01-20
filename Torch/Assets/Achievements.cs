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

	// // // // // // // // // Achievements
	public bool tutorial_done;
	public bool spidernest_destroyed;
	public bool wizarnest_destroyed;
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

	public Vector2  hiddenTarget;
	public Vector2 shownTarget;
	Vector2 target;
	Vector2 damping;

	// // // // // // // // // Reset
	void Awake () {
		AchievenmentTest = false;

		tutorial_done = false;			//x doet het!
		spidernest_destroyed = false;	//x doet het!
		wizarnest_destroyed = false;	//x
		boss_defeated = false;			//x works
		keys_collected_100 = false;		//x works
		all_shopitems_bought = false;	//x works
		home_stairs_climbed = false;
		died_20_times = false;			//x doet het!
		fell_50_times = false;			//x doet het!
		playtime_1_hour = false;		
		km_42_walked = false;			//x works
		powerups_50_used = false;		//x
		cheats_unlocked = false;		//x

		enemies_killed_200 = false;		//x works
		spider_100 = false;				//x works
		bomber_100 = false;				//x works
		minota_100 = false;				//x works
		ghost_100 = false;				//x works

		levels_10 = false;				//x works 
		levels_20 = false;				//x works
		levels_30 = false;				//x works
		levels_40 = false;				//x works

		timesfallen = 0;
		timesdied = 0;
		totalcollectedkeys = 0;
		totalpowerupsused = 0;

		target = hiddenTarget;
	}

	void Update () {
		if (Input.GetKey ("x")) {
			NewAchievement ("test");
		}
		Aimage.rectTransform.anchoredPosition = Vector2.MoveTowards (Aimage.rectTransform.anchoredPosition, target, 5);
	}

	public void NewAchievement (string text) {
		Debug.Log("New Achievement: " + text);
		Atext.text = "New Achievement: " + text;
		StartCoroutine ("WaitSecs");
		AchievementsPanel.Instance.SetAchievement (text);
	}

	IEnumerator WaitSecs () {
		target = shownTarget;

		yield return new WaitForSeconds (2);

		target = hiddenTarget;
	}

	public void diedAchievement() {
		timesdied++;
		if (!died_20_times && timesdied >= 20) {
			died_20_times = true;
			NewAchievement ("Congratulations, you've died 50 times!");
		}
	}

	public void fallenAchievement () {
		timesfallen++;
		if (!fell_50_times && timesfallen >= 50) {
			fell_50_times = true;
			NewAchievement ("Congratulations, you've fallen 50 times!");
		}
	}

	public void bossAchievement () {
		if (!boss_defeated) {
			boss_defeated = true;
			NewAchievement ("First boss killed!");
		}
	}

	public void nestAchievement(string thisname) {
		switch (thisname) {
		case "SPIDERNEST":
			if (!spidernest_destroyed) {
				spidernest_destroyed = true;
				NewAchievement ("First spidernest destroyed!");
			}
			break;
		case "WIZARDNEST":
			if (!wizarnest_destroyed) {
				wizarnest_destroyed = true;
				NewAchievement ("First wizardnest destroyed!");
			}
			break;
		}
	}

	public void cheatsAchievement () {
		cheats_unlocked = true;
		NewAchievement ("Cheats unlocked!");
	}

	public void powerups50Achievement() {
		totalpowerupsused++;
		if (!powerups_50_used && totalpowerupsused >= 50) {
			powerups_50_used = true;
			NewAchievement ("50 powerups used!");
		}
	}

	public void km42Achievement() {
		km_42_walked = true;
		NewAchievement ("You've walked 4200 m in one level!");
	}

	public void shopAchievement() {
		all_shopitems_bought = true;
		NewAchievement ("You own all shop items!");
	}

	public void enemiesAchievement() {
		if (!enemies_killed_200) {
			enemies_killed_200 = true;
			NewAchievement ("You've killed 200 enemies!");
		}
	}

	public void tutortialAchievement () {
		if (!tutorial_done) {
			tutorial_done = true;
			NewAchievement ("Tutorial completed!");
		}
	}

	public void collectedKeys () {
		totalcollectedkeys++;
		if (!keys_collected_100 && totalcollectedkeys >= 100) {
			keys_collected_100 = true;
			NewAchievement ("You've collected 100 keys!");
		}
	}

	public void enemiesAchievement (string name) {
		switch (name) {
		case "spider":
			spidersKilled++;
			if (!spider_100 && spidersKilled >= 100) {
				spider_100 = true;
				NewAchievement ("You've killed 100 spiders!");
			}
			break;
		case "minotaur":
			minotaurKilled++;
			if (!minota_100 && minotaurKilled >= 100) {
				minota_100 = true;
				NewAchievement ("You've killed 100 minotaurs!");
			}
			break;
		case "ghost":
			ghostsKilled++;
			if (!ghost_100 && ghostsKilled >= 100) {
				ghost_100 = true;
				NewAchievement ("You've killed 100 ghosts!");
			}
			break;
		case "bomber":
			bomberKilled++;
			if (!bomber_100 && bomberKilled >= 100) {
				bomber_100 = true;
				NewAchievement ("You've killed 100 bombers!");
			}
			break;
		}
	}

	public void levelAchievement (int level) {
		if (level >= 10 && !levels_10) {
			levels_10 = true;
			NewAchievement ("Level 10 reached!");
		}
		if (level >= 20 && !levels_20) {
			levels_20 = true;			
			NewAchievement ("Level 20 reached!");
		}
		if (level >= 30 && !levels_30) {
			levels_30 = true;
			NewAchievement ("Level 30 reached!");
		}
		if (level >= 40 && !levels_30) {
			levels_40 = true;
			NewAchievement ("Level 40 reached!");
		}
	}

}
