using UnityEngine;
using System.Collections;

public class SwitchLever : MonoBehaviour {

	public bool is_on = false;

	public void flipSwitch() {
		if (is_on) {
			transform.Rotate (0, 0, -60);
		}
		else {
			transform.Rotate (0, 0, 60);
		}
		is_on = !is_on;
	}

}
