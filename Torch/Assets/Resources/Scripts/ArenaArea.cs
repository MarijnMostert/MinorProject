using UnityEngine;
using System.Collections;

public class ArenaArea : MonoBehaviour {

	public bool playerinarea = false;
	public string AreaName = "Default Area Name";
	// Use this for initialization
	void Start () {
	
	}
	

	void OnTriggerStay (Collider other) {
		if (other.CompareTag ("Player")) {
			playerinarea = true;
		}
	}
	
	void OnDisable (){
		playerinarea = false;
	}
}
