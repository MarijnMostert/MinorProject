using UnityEngine;
using System.Collections;

public class PlayerDamagable : MonoBehaviour, IDamagable {
	public float positionIsSavedEveryXSeconds;

	private Vector3 lastKnownPosition;

	void Update(){
		
		if (transform.position.y < -12) {
			Die ();
		}
	}

	public void takeDamage(int damage, bool crit){
		//Debug.Log ("Player is taking damage");
		//GameObject torch = transform.FindChild ("Torch").gameObject;
		if (hasTorch ()) {
			gameObject.GetComponentInChildren<Torch> ().takeDamage (damage, crit);
		}
	}
	public void saveRespawnPosition(){
		lastKnownPosition = transform.position;
		Debug.Log ("lastKnownPosition: " + lastKnownPosition);
	}
	public void Die(){
		transform.position = lastKnownPosition;
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
