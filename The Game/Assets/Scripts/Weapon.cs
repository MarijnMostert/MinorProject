using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Weapon : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void fireRangedWeapon(){
		Debug.Log ("fire");
	}
}
