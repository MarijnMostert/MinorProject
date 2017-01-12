using UnityEngine;
using System.Collections;

public class RotateTarget : MonoBehaviour {

	Quaternion target;
	float length;
	SwitchGround parental;

	void Start () {
		length = 3.0f;
		parental = GetComponentInParent<SwitchGround> ();
	}

	public void Rotate (Vector3 direction) {

		Vector3 relativepos = transform.parent.position - transform.position - Vector3.up;
		Vector3 inproduct = Vector3.Cross (direction, direction);

		Debug.Log (relativepos);

		float rotation = inproduct.y;

		Quaternion qRotation = Quaternion.AngleAxis (rotation * length, Vector3.up);
		Quaternion current = transform.parent.localRotation;
		target = current * qRotation;

		parental.setTarget (target);
	}
}
