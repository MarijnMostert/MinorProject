using UnityEngine;
using System.Collections;

public class ForceFieldLightUp : MonoBehaviour {

	private Renderer rend;
	public bool change = false;

	void OnTriggerEnter(Collider other){
		rend.enabled = true;
	}

	void OnTriggerExit(Collider other){
		rend.enabled = false;
	}

	void Start(){
		rend = GetComponent<Renderer> ();
		rend.enabled = false;
	}

	public void Destroy(){
		//GameObject.Find ("ForceFieldColor").GetComponent<ForceFieldTriggerLightUp> ().FlashRed ();
		Destroy (gameObject);
	}
}
