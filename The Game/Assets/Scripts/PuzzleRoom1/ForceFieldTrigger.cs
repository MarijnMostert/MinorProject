using UnityEngine;
using System.Collections;

public class ForceFieldTrigger : MonoBehaviour {
	
		public int counter = 0;

		void OnTriggerEnter(Collider other){
			if (GameObject.Find ("ForceField") != null) {
				counter++;
				GameObject.Find ("ForceFieldColor").GetComponent<ForceFieldTriggerLightUp> ().FlashGreen ();
			}
			if (counter == 5) {
			GameObject.Find ("MovingWalls").GetComponent<CommonWallScript> ().StartWalls();
				GameObject.Find ("ForceField").GetComponent<ForceFieldLightUp> ().Destroy ();
			}
		}
	}
