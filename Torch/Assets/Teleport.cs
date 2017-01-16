using UnityEngine;
using System.Collections;

public class Teleport : InteractableItem {

	public Transform targetTransform;

	public override void action(GameObject triggerObject){
		triggerObject.transform.position = targetTransform.position;
	}
}
