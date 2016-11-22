using UnityEngine;
using System.Collections;

public class CommonWallScript : MonoBehaviour {

	public void StartWalls () {
		GameObject.Find ("MovingWall").GetComponent<WallMover> ().start = true;
		GameObject.Find ("MovingWall (1)").GetComponent<WallMover> ().start = true;
		GameObject.Find ("MovingWall (2)").GetComponent<WallMover> ().start = true;
		GameObject.Find ("MovingWall (3)").GetComponent<WallMover> ().start = true;
		GameObject.Find ("MovingWall (4)").GetComponent<WallMover> ().start = true;
		GameObject.Find ("MovingWall (5)").GetComponent<WallMover> ().start = true;
		GameObject.Find ("MovingWall (6)").GetComponent<WallMover> ().start = true;
		GameObject.Find ("MovingWall (7)").GetComponent<WallMover> ().start = true;
		GameObject.Find ("MovingWall (8)").GetComponent<WallMover> ().start = true;
	}

}
