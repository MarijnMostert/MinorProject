using UnityEngine;
using System.Collections;

public class BlockPuzzleRoom : MonoBehaviour {

	public PuzzleBlockPad Pad1, Pad2, Pad3;
	public GameObject Block1, Block2, Block3;
	public bool activated;

	void Start(){
		Vector3 positionPad1, positionPad2, positionPad3, positionBlock1, positionBlock2, positionBlock3;
		positionPad1 = new Vector3 (Random.Range (-12f, 12f), 0.01f, Random.Range (-12f, 12f));
		positionPad2 = positionPad1;
		while ((positionPad1 - positionPad2).magnitude < 4f) {
			positionPad2 = new Vector3 (Random.Range (-12f, 12f), 0.01f, Random.Range (-12f, 12f));
		}
		positionPad3 = positionPad2;
		while ((positionPad1 - positionPad3).magnitude < 4f || (positionPad2 - positionPad3).magnitude < 4f) {
			positionPad3 = new Vector3 (Random.Range (-12f, 12f), 0.01f, Random.Range (-12f, 12f));
		}
		Pad1 = Instantiate (Pad1, transform) as PuzzleBlockPad;
		Pad2 = Instantiate (Pad2, transform) as PuzzleBlockPad;
		Pad3 = Instantiate (Pad3, transform) as PuzzleBlockPad;
		Pad1.transform.localPosition = positionPad1;
		Pad2.transform.localPosition = positionPad2;
		Pad3.transform.localPosition = positionPad3;

		positionBlock1 = new Vector3 (Random.Range (-12f, 12f), 1.01f, Random.Range (-12f, 12f));
		positionBlock2 = new Vector3 (Random.Range (-12f, 12f), 1.01f, Random.Range (-12f, 12f));
		positionBlock3 = new Vector3 (Random.Range (-12f, 12f), 1.01f, Random.Range (-12f, 12f));
		Block1 = Instantiate (Block1, transform) as GameObject;
		Block2 = Instantiate (Block2, transform) as GameObject;
		Block3 = Instantiate (Block3, transform) as GameObject;
		Block1.transform.localPosition = positionBlock1;
		Block2.transform.localPosition = positionBlock2;
		Block3.transform.localPosition = positionBlock3;
		Block1.name = "Block1";
		Block2.name = "Block2";
		Block3.name = "Block3";

		DeactivateLever ();
	}

	void Update(){
		if(!activated){
			if (Pad1.activated && Pad2.activated && Pad3.activated) {
				activated = true;
				Debug.Log ("Puzzle room cleared. Open doors");
				ActivateLever ();
				//Opening of the doors
				//Fireworks!
			}
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			Debug.Log ("Player entered Block Puzzle Room");

		}
	}

	public void DeactivateLever(){
		gameObject.GetComponentInChildren<LeverActivator> ().Deactivate ();
	}

	public void ActivateLever(){
		gameObject.GetComponentInChildren<LeverActivator> (true).Activate ();
	}
}
