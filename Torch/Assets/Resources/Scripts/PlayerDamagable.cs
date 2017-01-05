using UnityEngine;
using System.Collections;

public class PlayerDamagable : MonoBehaviour, IDamagable {
	public float positionIsSavedEveryXSeconds;

	private Vector3 lastKnownPosition;

	void Start () {
		StartCoroutine (respawnPosition());
	}
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
	private IEnumerator respawnPosition(){
		while (true) {
			if (transform.position.y>=0.24f){
				lastKnownPosition = transform.position;
				Debug.Log ("lastKnownPosition: " + lastKnownPosition);
			}
			yield return new WaitForSeconds (positionIsSavedEveryXSeconds);
		}
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
