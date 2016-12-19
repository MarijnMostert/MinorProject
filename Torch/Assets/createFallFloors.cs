using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class createFallFloors : MonoBehaviour {

	GameObject blocks;
	GameObject platform;
	List<Vector3> places;

	// Use this for initialization
	void Start () {
		blocks = new GameObject("AllPlatforms");
		blocks.transform.SetParent (transform);
		platform = Resources.Load("Prefabs/PuzzlesScenes/FallPlatform", typeof(GameObject)) as GameObject;

		makeList ();
		makeBlocks ();

		blocks.transform.localPosition = Vector3.zero;

		GetComponent<AllTimes> ().Restart ();
	}

	void makeBlocks () {
		foreach (Vector3 place in places) {
			GameObject floor = GameObject.Instantiate (platform, place, Quaternion.identity, blocks.transform) as GameObject;
		}
	}

	void makeList() {
		places = new List<Vector3> ();
		int[] array = { 13, 13, 13, 12, 11, 11, 10, 9, 8, 7, 6, 5, 4 };

		int j = 12;
		for (int i = 0; i < array.Length; i++) {
			places.Add (new Vector3 (array [i], 0, array [j]));
			places.Add (new Vector3 (-array [i], 0, array [j]));
			places.Add (new Vector3 (array [i], 0, -array [j]));
			places.Add (new Vector3 (-array [i], 0, -array [j]));
			j--;
		}
	}
}