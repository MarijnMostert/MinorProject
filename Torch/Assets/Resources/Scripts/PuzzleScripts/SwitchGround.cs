using UnityEngine;
using System.Collections;

public class SwitchGround : MonoBehaviour {

	public int degrees;
	LeverActivator lever;

	private bool past;
	private bool current;

	void Start () {
		lever = GetComponent<myLever> ().lever;
	}

	void Update () {
		current = lever.is_on;
		if (current != past) {
			transform.Rotate (0, degrees, 0);
		}
		past = current;
	}

}
