using UnityEngine;
using System.Collections;

public class backtoHomeScreen : InteractableItem {

	public override void action(GameObject triggerObject){
		gameManager.TransitionDeathToMain ();
	}
}
