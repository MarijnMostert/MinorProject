using UnityEngine;
using System.Collections;

public class LaserRoom : MonoBehaviour {

	public LeverActivator lever;
	bool finished = false;
	bool previous = false;

	// Use this for initialization
	void Start () {
		lever = GetComponent<myLever> ().lever;
	}
	
	// Update is called once per frame
	void Update () {
		if (!finished && previous != lever.is_on && lever.is_on) {
			finished = true;
			Laserbeam[] lasers = GetComponentsInChildren<Laserbeam> ();
			LineRenderer[] renderers = GetComponentsInChildren<LineRenderer>();

			foreach (Laserbeam laser in lasers) {
				Debug.Log ("found 1");
				laser.active = false;
			}
			foreach (LineRenderer renderer in renderers) {
				renderer.enabled = false;
			}
		}
		previous = lever.is_on;
	}
}
