using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour {

	private GameObject player;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void LateUpdate(){
		gameObject.transform.position = new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z);
	}
		
		
}
