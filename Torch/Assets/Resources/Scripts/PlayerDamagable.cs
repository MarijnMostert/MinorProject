using UnityEngine;
using System.Collections;

public class PlayerDamagable : MonoBehaviour, IDamagable {

	public void takeDamage(int damage){
		//GameObject torch = transform.FindChild ("Torch").gameObject;
		if (hasTorch()) {
			transform.FindChild("Torch").GetComponent<Torch> ().takeDamage (damage);
		}
	}

	public void Die(){
		//Nothing
	}
		
	bool hasTorch(){
		Transform[] transforms = GetComponentsInChildren<Transform>();
		foreach(Transform t in transforms)
		{
			if(t.gameObject.name == "Torch"){
				return true;
			}
		}
		return false;
	}
}
