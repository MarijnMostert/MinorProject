using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class createMovingFloors : MonoBehaviour {

	GameObject blocks;
	GameObject platform;
	List<Vector3> placesx;
	List<Vector3> placesz;


	// Use this for initialization
	void Start () {
		blocks = new GameObject("AllPlatforms");
		blocks.transform.SetParent (transform);
		platform = Resources.Load("Prefabs/PuzzlesScenes/MovingPlatform", typeof(GameObject)) as GameObject;

		makeList ();
		makeBlocks ();

		blocks.transform.localPosition = Vector3.zero;

		GetComponentInParent<AllSpeeds> ().Restart ();
	}

	void makeBlocks () {
		foreach (Vector3 place in placesx) {
			GameObject floor = GameObject.Instantiate (platform, place, Quaternion.identity, blocks.transform) as GameObject;
			floor.GetComponent<PingPongPlatform> ().choice = 'z';
		}

		foreach (Vector3 place in placesz) {
			Debug.Log (place);
			GameObject floor = GameObject.Instantiate (platform, place, Quaternion.identity, blocks.transform) as GameObject;
			floor.GetComponent<PingPongPlatform> ().choice = 'x';
		}
	}

	void makeList() {
		placesx = new List<Vector3> ();
		placesz = new List<Vector3> ();
		float[] array = { 4f, 5.5f, 7f, 8.5f, 10f };

		for (int i = 0; i < array.Length; i++) {
			placesx.Add (new Vector3 (array [i], 0, 0));
			placesx.Add (new Vector3 (-array [i], 0, 0));
			placesz.Add (new Vector3 (0, 0, array [i]));
			placesz.Add (new Vector3 (0, 0, -array [i]));
		}
	}
}