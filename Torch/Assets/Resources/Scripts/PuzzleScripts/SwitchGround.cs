using UnityEngine;
using System.Collections;

public class SwitchGround : MonoBehaviour {

	public GameObject lever;
	public int degrees;

	private bool past;
	private bool current;

	void Start () {
		current = lever.GetComponent<LeverActivator> ().is_on;
	}

	void Update () {
		current = lever.GetComponent<LeverActivator> ().is_on;
		if (current != past) {
			transform.Rotate (0, degrees, 0);
		}
		past = current;
	}

}
