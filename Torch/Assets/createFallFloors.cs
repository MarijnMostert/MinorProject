using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class createFallFloors : MonoBehaviour {

	GameObject blocks;
	GameObject platform;
	List<Vector3> places;
	LeverActivator lever { get; set; }
	bool current = false;
	List<GameObject> fallingblocks;
	bool finished = false;

	// Use this for initialization
	void Start () {
		fallingblocks = new List<GameObject> ();
		blocks = new GameObject("AllPlatforms");
		blocks.transform.SetParent (transform);
		platform = Resources.Load("Prefabs/PuzzlesScenes/FallPlatform", typeof(GameObject)) as GameObject;
		lever = GetComponent<myLever> ().lever;

		makeList ();
		makeBlocks ();

		blocks.transform.localPosition = Vector3.zero;

		GetComponent<AllTimes> ().Restart ();
	}

	void Update () {
		if (!finished && current != lever.is_on && lever.is_on) {
			finished = true;
			foreach (GameObject block in fallingblocks) {
				block.SetActive (false);
			}
			platform.GetComponent<FallPlatform> ().active = false;
			Vector3 counter = new Vector3 (4.0f, 11.0f, 1.5f);
			buildOppositeX (counter, false);
			buildOppositeZ (counter, false);
			counter *= -1;
			buildOppositeZ (counter, true);
			buildOppositeX (counter, true);
			platform.GetComponent<FallPlatform> ().active = true;
		}
		current = lever.is_on;
	}

	void makeBlocks () {
		foreach (Vector3 place in places) {
			GameObject floor = GameObject.Instantiate (platform, place, Quaternion.identity, blocks.transform) as GameObject;
			fallingblocks.Add (floor);
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

	void OnTriggerEnter (Collider other) {
		if(other.gameObject.CompareTag("Player")) {
			Vector3 relativepos = other.gameObject.transform.position - transform.position;
			Debug.Log (relativepos.x + " " + relativepos.y + " " + relativepos.z);
			makeOppositeBlocks (relativepos);
		}
	}

	public void makeOppositeBlocks (Vector3 relativepos) {
		Vector3 counter = new Vector3 (4.0f, 11.0f, 1.5f);
		bool negative = false;

		if (relativepos.x > 12 || relativepos.z > 12) {
			counter *= -1;
			negative = true;
		}
		if (relativepos.x < -12 || relativepos.x > 12) {
			buildOppositeX (counter, negative);
		} else if (relativepos.z < -12 || relativepos.z > 12) {
			buildOppositeZ (counter, negative);
		}
	}

	void buildOppositeX(Vector3 counter, bool negative) {
		float i = counter.x;
		while ((!negative && i < counter.y) || (negative && i > counter.y)) {
			Vector3 place = transform.position + new Vector3 (i, 0, 0);
			GameObject block = GameObject.Instantiate (platform, place, Quaternion.identity, blocks.transform) as GameObject;
			fallingblocks.Add (block);
			i += counter.z;
		}
	}
	void buildOppositeZ(Vector3 counter, bool negative) {
		float i = counter.x;
		while ((!negative && i < counter.y) || (negative && i > counter.y)) {
			Vector3 place = transform.position + new Vector3 (0, 0, i);
			GameObject block = GameObject.Instantiate (platform, place, Quaternion.identity, blocks.transform) as GameObject;
			fallingblocks.Add (block);
			i += counter.z;
		}
	}

}