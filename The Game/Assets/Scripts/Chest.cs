using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Chest : MonoBehaviour {

	public GameObject[] contents;
	public GameObject chestInstructionPopUp;
	public float floatingHeight = 2f;

	private Camera cam;
	private GameObject canvas;
	private string interactionButton;

	void Start () {
		canvas = Instantiate (chestInstructionPopUp, new Vector3(transform.position.x, floatingHeight, transform.position.z), transform.rotation) as GameObject;
		canvas.SetActive (false);
		cam = Camera.main; 
		interactionButton = "InteractionButton";
	}

	void LateUpdate() {
		lookAtCamera (canvas, cam);
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			canvas.SetActive (true);
			if (Input.GetButtonDown (interactionButton)) {
				for(int i = 0; i < contents.Length; i++){
					flyOut (contents [i]);
				}
				Destroy (gameObject);
				Destroy (canvas);
			}
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

	void flyOut(GameObject obj){
		float randomX = (1f - 2f * Random.value) * 2;
		float randomZ = (1f - 2f * Random.value) * 2;
		Vector3 spawnLocation = new Vector3(transform.position.x + randomX, -.5f, transform.position.z + randomZ);
		Instantiate (obj, spawnLocation, transform.rotation);
	}
}