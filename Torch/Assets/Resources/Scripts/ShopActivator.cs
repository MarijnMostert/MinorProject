using UnityEngine;
using System.Collections;

public class ShopActivator : InteractableItem {

	public override void action(GameObject TriggerObject){
		gameManager.ToggleShop ();
	}
}
