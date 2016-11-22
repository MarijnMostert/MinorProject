using UnityEngine;
using System.Collections;

public class ForceFieldLightUp : MonoBehaviour {

	private Renderer rend;

	void OnTriggerStay(Collider other){
		rend.enabled = true;
		//wait a second or something
		//rend.enabled = false;
	}

	void Start(){
		rend = GetComponent<Renderer> ();
		rend.enabled = false;
	}


		
}
