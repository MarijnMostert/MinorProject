using UnityEngine;
using System.Collections;

public class AchievementStairs_a : MonoBehaviour {

	public int number;
	public AchievementStairs myparent;

	void Start () {
		myparent = transform.parent.GetComponent<AchievementStairs> ();
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			myparent.setPrevious(number);
			//Debug.Log ("woop");
		}
	}

}
