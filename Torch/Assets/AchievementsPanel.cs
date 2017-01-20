using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AchievementsPanel : MonoBehaviour {

	public static AchievementsPanel Instance;
	public Text[] achievements;
	public bool[] activated;

	void Awake(){
		Instance = this;
	}


	public void SetAchievement(string achievement){
		for(int i = 0; i < achievements.Length; i++){
			if (!activated [i]) {
				achievements [i].text = achievement;
				achievements [i].color = Color.yellow;
				activated[i] = true;
				break;
			}
		}
	}
}
