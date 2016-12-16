using UnityEngine;
using System.Collections;

public class SwitchGround : MonoBehaviour {

	public GameObject toBeSwitched;
	public Vector3 rotation;

	public void rotateGround () {
		toBeSwitched.transform.Rotate (rotation);
	}

}
