using UnityEngine;
using System.Collections;

public class Fallspeed : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
		TriggerActivator [] triggers = FindObjectsOfType(typeof(TriggerActivator)) as TriggerActivator [];
		foreach (var trigger in triggers) {
			trigger.setTimeLeft (speed);
		}
	}
}
