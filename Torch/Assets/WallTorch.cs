using UnityEngine;
using System.Collections;

public class WallTorch : MonoBehaviour {

	public GameObject[] WallTorches;

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			foreach (GameObject obj in WallTorches) {
				for (int i = 0; i < 3; i++) {
					obj.transform.GetChild (i).gameObject.SetActive (true);
				}
			}
		}
	}
}
