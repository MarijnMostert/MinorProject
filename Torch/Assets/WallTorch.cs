using UnityEngine;
using System.Collections;

public class WallTorch : MonoBehaviour {

	public GameObject[] WallTorches;
	private bool activated = false;
	public GameObject smokeParticles;

	void Start(){
		GameManager.Instance.addHighQualityItem (smokeParticles);
	}

	void OnTriggerEnter(Collider other){
		if (!activated && other.gameObject.CompareTag ("Player")) {
			for (int i = 0; i < WallTorches.Length; i++) {
				WallTorches [i].transform.GetChild (0).gameObject.SetActive (true);
			}
			/*
			foreach (GameObject obj in WallTorches) {
				for (int i = 0; i < 3; i++) {
					if (i != 1) {
						obj.transform.GetChild (i).gameObject.SetActive (true);
					} else {
						if (GameManager.Instance.data.highQuality)
							obj.transform.GetChild (i).gameObject.SetActive (true);
					}
				}
			}
			*/
			activated = true;
		}
	}
}
