using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {

	public float rotationSpeed = 1f;
	public float hoverAmplitude = 2f;
	public float hoverSpeed = 2f;

	protected float hoverHeight;

	protected virtual void Start () {
		hoverHeight = transform.position.y;
	}
	
	protected virtual void Update () {
		hover ();
	}

	//Hovers the object in a smooth motion
	public virtual void hover(){
		transform.position = new Vector3(transform.position.x, 1f/10f * hoverAmplitude * Mathf.Sin (Time.time * hoverSpeed) + hoverHeight, transform.position.z);
	}

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
