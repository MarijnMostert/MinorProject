using UnityEngine;
using System.Collections;

public class SwitchGround : MonoBehaviour {

	public LeverActivator lever;
	public int degrees;

	private bool past;
	private bool current;

	void Update () {
		current = lever.is_on;
		if (current != past) {
			transform.Rotate (0, degrees, 0);
		}
		past = current;
	}

	public void Reread () {
		lever = GetComponentInChildren<LeverActivator> ();
	}

}
