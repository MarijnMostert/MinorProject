using UnityEngine;
using System.Collections;

public class BallWallDown : MonoBehaviour {


	public void Down () {
		//if (move == true) {
			transform.Translate (0, -20, 0);
		//	move = false;
		//}
	}

	public void Up() {
		//if (move == true) {
		transform.Translate (0, 20, 0);
		//	move = false;
		//}
	}

}
