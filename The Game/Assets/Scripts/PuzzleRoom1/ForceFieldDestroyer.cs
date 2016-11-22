using UnityEngine;
using System.Collections;

public class ForceFieldDestroyer : MonoBehaviour {

	public void Destroy(){
		//GameObject.Find ("ForceFieldColor").GetComponent<ForceFieldTriggerLightUp> ().FlashRed ();
		Destroy (gameObject);
	}
}
