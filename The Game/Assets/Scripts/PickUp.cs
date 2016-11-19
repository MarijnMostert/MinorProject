using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {

	public float rotationSpeed = 1f;

	//Rotates the object in Z axis
	public virtual void rotateZ(){
		transform.Rotate (new Vector3 (0, 0, rotationSpeed));
	}

	//Rotates the object in Y axis
	public virtual void rotateY(){
		transform.Rotate (new Vector3 (0, rotationSpeed, 0));
	}

	//Rotates the object in X axis
	public virtual void rotateX(){
		transform.Rotate (new Vector3 (rotationSpeed, 0, 0));
	}



}
