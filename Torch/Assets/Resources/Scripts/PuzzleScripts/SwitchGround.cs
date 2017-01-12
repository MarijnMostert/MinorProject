using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwitchGround : MonoBehaviour {

	public float speed;
	public GameObject Rotor;
	private Quaternion target;

	void Start () {
		target = Rotor.transform.rotation;
	}

	void Update () {
		Quaternion temp = Rotor.transform.rotation;
		Rotor.transform.rotation = Quaternion.Slerp (Rotor.transform.rotation, target, 0.15f);
	}

	public void setTarget(Quaternion trgt) {
		Debug.Log ("Target is now " + trgt.eulerAngles.ToString ());
		target = trgt;
	}
}


