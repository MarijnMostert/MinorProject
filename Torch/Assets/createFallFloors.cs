using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class createFallFloors : MonoBehaviour {

	GameObject blocks;
	GameObject platform;
	List<Vector3> places;
	LeverActivator lever { get; set; }
	bool current = false;
	bool started = false;
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
			Vector3 counter = new Vector3 (4.0f, 11.0f, 2.0f);
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

		List<int[]> x_options = new List<int[]> ();
		List<int[]> y_options = new List<int[]> ();

		int[] optionOne_x = { 4, 6, 8, 10, 12, 12, 12, 10, 8, 8, 8, 10, 12 };
		int[] optionOne_y = { 12, 12, 12, 12, 12, 10, 8, 8, 8, 6, 4, 4, 4 };

		int[] optionTwo_x = { 14, 14, 14, 14, 12, 10, 10, 8, 6, 6, 6, 4 };
		int[] optionTwo_y = { 4, 6, 8, 10, 10, 10, 8, 8, 8, 10, 12, 12 };

		int[] optionThr_x = { 4, 6, 8, 10, 10, 12, 14, 14, 14, 12, 12 };
		int[] optionThr_y = { 12, 12, 12, 12, 10, 10, 10, 8, 6, 6, 4 };

		int[] optionFou_x = { 4, 6, 8, 10, 12, 12, 12, 10, 8, 8, 8, 10, 12, 12 };
		int[] optionFou_y = { 14, 14, 14, 14, 14, 12, 10, 10, 10, 8, 6, 6, 6, 4 };

		x_options.Add (optionOne_x); 
		x_options.Add (optionTwo_x); 
		x_options.Add (optionThr_x); 
		x_options.Add (optionFou_x);
		y_options.Add (optionOne_y); 
		y_options.Add (optionTwo_y); 
		y_options.Add (optionThr_y); 
		y_options.Add (optionFou_y);

		for (int i = 0; i < 4; i++) {
			int number = Random.Range (0, 4);
			int xfactor = 1;
			int zfactor = 1;
			if (i%2 == 0)
				xfactor = -1;
			if (i > 1)
				zfactor = -1;
			int[] thisx = x_options [number];
			int[] thisy = y_options [number];

			for (int j = 0; j < thisx.Length; j++) {
				places.Add(new Vector3(thisx[j] * xfactor, 0, thisy[j]* zfactor));
			}
		}
	}

	void OnTriggerEnter (Collider other) {
		if (!started) {
			if (other.gameObject.CompareTag ("Player")) {
				Vector3 relativepos = other.gameObject.transform.position - transform.position;
//			Debug.Log (relativepos.x + " " + relativepos.y + " " + relativepos.z);
				makeOppositeBlocks (relativepos);
				started = true;
			}
		}
	}

	public void makeOppositeBlocks (Vector3 relativepos) {
		Vector3 counter = new Vector3 (4.0f, 11.0f, 2f);
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