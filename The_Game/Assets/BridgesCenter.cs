using UnityEngine;
using System.Collections;

public class BridgesCenter : MonoBehaviour {

	public Vector3 coordinates;

	void Start () {
		coordinates = transform.position;
		GetComponentInChildren<LeverActivator> ().SetCenter (coordinates);
	}

}
