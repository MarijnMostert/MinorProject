using UnityEngine;
using System.Collections;

public class Doors : MonoBehaviour {

	Animator animator;
	bool doorOpen;

	void Start()
	{
		doorOpen = false;
		animator = GetComponent<Animator>();
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Player")) {
			doorOpen = true;
			DoorController ("Open");
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (doorOpen) {
			doorOpen = false;
			DoorController ("Close");
		}
	}

	void DoorController(string direction)
	{
		animator.SetTrigger (direction);
	}
}