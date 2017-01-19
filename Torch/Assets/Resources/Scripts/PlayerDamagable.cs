using UnityEngine;
using System.Collections;

public class PlayerDamagable : MonoBehaviour, IDamagable {
	public GameManager gameManager;
	private Vector3 lastKnownPosition;

	void Start(){
		gameManager = GameManager.Instance;
	}

	void Update(){
		if (transform.position.y < -12) {
			gameManager.achievements.fallenAchievement ();
			Respawn ();
		}
	}

	public void takeDamage(int damage, bool crit, GameObject source){
		//Debug.Log ("Player is taking damage");
		//GameObject torch = transform.FindChild ("Torch").gameObject;
		if (hasTorch ()) {
			gameObject.GetComponentInChildren<Torch> ().takeDamage (damage, crit, source);
		}
	}

	public void saveRespawnPosition(){
		lastKnownPosition = transform.position;
		gameManager.RespawnPosition = lastKnownPosition;
		Debug.Log ("lastKnownPosition: " + lastKnownPosition);
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject player in players) {
			player.transform.position = gameManager.RespawnPosition;
		}
	}

	public void Die(){
	}

	public void Respawn(){
		transform.position = gameManager.RespawnPosition;
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
