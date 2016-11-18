using UnityEngine;
using System.Collections;

public class WallDestroyer : MonoBehaviour {

	private Vector3 PlayerPos;
	void Start(){
		
	}

	// Update is called once per frame
	void Update () {
		PlayerPos = GameObject.Find("Player").transform.position;
		if (PlayerPos.x > 20.0f){
			Destroy(gameObject);
		}
	}
}
