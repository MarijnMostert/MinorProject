using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Chest : MonoBehaviour {

	public GameObject[] contents;
	public GameObject chestInstructionPopUp;
	public float floatingHeight = 2f;

	private Camera cam;
	private GameObject canvas;

	void Start () {
		canvas = Instantiate (chestInstructionPopUp, new Vector3(transform.position.x, floatingHeight, transform.position.z), transform.rotation) as GameObject;
		canvas.SetActive (false);
		cam = Camera.main; 
	}

	void LateUpdate() {
		lookAtCamera (canvas, cam);
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			canvas.SetActive (true);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			canvas.SetActive (false);
		}
	}

	void lookAtCamera(GameObject obj, Camera cam){
		obj.transform.rotation = cam.transform.rotation;
	}
}