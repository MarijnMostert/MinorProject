using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	public float rotationSpeedX;
	public float rotationSpeedY;
	public float rotationSpeedZ;

	void Start(){
		rotationSpeedX *= 5f;
		rotationSpeedY *= 5f;
		rotationSpeedZ *= 5f;
	}

	void Update () {
		transform.Rotate (new Vector3 (rotationSpeedX, rotationSpeedY, rotationSpeedZ) * Time.deltaTime);
	}
}
