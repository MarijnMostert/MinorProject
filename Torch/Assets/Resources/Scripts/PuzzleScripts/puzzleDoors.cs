using UnityEngine;
using System.Collections;

public class puzzleDoors : MonoBehaviour {

	public string RoomType;
	public bool done = false;
	public bool locked = false;
	public bool active = false;
	bool prev = false;
	Doors[] doors;
	GameManager gameManager;
	public LeverActivator myLever;
	private float StartTime;
	public GameObject key;

	// Use this for initialization
	void Start () {
		doors = GetComponentsInChildren<Doors> ();
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
	}

	void Update () {
		done = myLever.is_on;
		if (prev != done && done) {
			EndPuzzle ();
		}
		prev = done;
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider other) {
		if (!done && !locked && other.gameObject.CompareTag("Player")) {
			StartPuzzle ();	
			gameManager.Roomtype = RoomType;
			gameManager.WritePuzzleStart ();
			locked = true;
		}
	}

	void StartPuzzle () {
		for (int i = 0; i < doors.Length; i++) {
			doors[i].Close ();
			doors[i].locked = true;
		}
		Debug.Log ("SPAWNER STOPPED");
		gameManager.spawner.dead = true;
		active = true;
		StartTime = Time.time;
		for (int i = 0; i < gameManager.playerManagers.Length; i++) {
			gameManager.playerManagers [i].playerInstance.GetComponent<Rigidbody> ().constraints &= ~RigidbodyConstraints.FreezePositionY;
		}
	}

	void EndPuzzle () {
		for (int i = 0; i < doors.Length; i++) {
			doors [i].Open ();
		}
		done = true;
		active = false;
		gameManager.spawner.dead = false;
		gameManager.WritePuzzleComplete (Time.time - StartTime);
		gameManager.Roomtype = null;

		//Instantiate a key
		Vector3 keyPosition = transform.position + new Vector3(0f, 1f, 0f);
		Instantiate (key, keyPosition, transform.rotation);
		Destroy (myLever.gameObject);
	}
}
