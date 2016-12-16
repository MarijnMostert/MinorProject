using UnityEngine;
using System.Collections;

public class EnterPuzzle : MonoBehaviour {

	public char direction;
	float rotation = 0;
	GameObject puzzles;
	public static bool made = false;

	void Start (){
		puzzles = (GameObject) Resources.Load ("Prefabs/PuzzlesScenes/Puzzleblocks", typeof(GameObject));

		switch (direction) {
		case 'n': rotation = 0; break;
		case 'e': rotation = 90; break;
		case 's': rotation = 180; break;
		case 'w': rotation = 270; break;
		}
	} // Sets how far the fall blocks must be rotated

	void OnTriggerEnter (Collider other) {
		if (!made && other.gameObject.CompareTag ("Player")) {
			Instantiate (puzzles, transform.parent.parent.parent.position, Quaternion.Euler (0, rotation, 0));
			made = true;
		}
	}
}
