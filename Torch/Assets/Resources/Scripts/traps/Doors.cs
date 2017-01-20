using UnityEngine;
using System.Collections;

public class Doors : MonoBehaviour {

	Animator animator;
	public bool doorOpen;
	public bool locked = false;

	public Animator getAnimator() {
		return animator;
	}

	void Start()
	{
		doorOpen = false;
		animator = GetComponent<Animator>();
	}


	void OnTriggerEnter(Collider other)
	{
		if (!locked) {
			if (other.gameObject.CompareTag ("Player")) {
				Open ();
			}
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (!locked) {
			if (other.gameObject.CompareTag ("Player")) {
				Open ();
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (!locked) {
			Close ();
		}
	}

	public void DoorController(string direction)
	{
		animator.SetTrigger (direction);
	}

	public void Close() {
		if (doorOpen) {
			doorOpen = false;
			DoorController ("Close");
		}
	}

	public void Open () {
		if (!doorOpen) {
			doorOpen = true;
			DoorController ("Open");
		}
	}
}