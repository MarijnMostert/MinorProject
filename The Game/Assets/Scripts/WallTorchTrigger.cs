using UnityEngine;
using System.Collections;

public class WallTorchTrigger : MonoBehaviour {

	public GameObject[] lights;

	// Use this for initialization
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			foreach (var light in lights) {
				light.SetActive (true);
			}
		}
	}
}
