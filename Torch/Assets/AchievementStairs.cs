using UnityEngine;
using System.Collections;

public class AchievementStairs : MonoBehaviour {

	public int beforeprevious = 0;
	public int previous = 0;
	public int current = 0;

	public void setPrevious (int number) {
		int current = number;

		if (beforeprevious == 1 && previous == 2 && number == 3) {
			GameManager.Instance.achievements.stairsAchievement ();
		}

		if (number != previous) {
			beforeprevious = previous;
			previous = number;
		}
	}
	


}
