using UnityEngine;
using System.Collections;

public class PlayerDamagable : MonoBehaviour, IDamagable {

	public void takeDamage(int damage){
		//Debug.Log ("Player is taking damage");
		//GameObject torch = transform.FindChild ("Torch").gameObject;
		if (hasTorch ()) {
			gameObject.GetComponentInChildren<Torch> ().takeDamage (damage);
		}
	}

	public void Die(){
		//Nothing
	}
		
	bool hasTorch(){
		Transform[] transforms = GetComponentsInChildren<Transform>();
		foreach(Transform t in transforms)
		{
			if(t.gameObject.CompareTag("Torch")){
				return true;
			}
		}
		return false;
	}
}
