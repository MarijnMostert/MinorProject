using UnityEngine;
using System.Collections;

public class ForceFieldTriggerLightUp : MonoBehaviour {

	public Renderer rend;
	private float timestamp;

	// Use this for initialization
	void Start(){
		rend = GetComponent<Renderer> ();
		rend.enabled = false;
	}

	public void FlashGreen () {
		rend.enabled = true;
		timestamp = Time.time;
	}

	public void FlashRed(){
		ChangeColor ();
		rend.enabled = true;
	}

	private void ChangeColor(){
		rend.material.color = Color.red;
	}


	void Update(){
		if (rend.enabled == true) {
			if ((float)Time.time > timestamp + 1.0f) {
				rend.enabled = false;
			}
		}	
	}
}
