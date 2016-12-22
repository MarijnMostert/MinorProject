using UnityEngine;
using System.Collections;

public class BossBlock : MonoBehaviour {

	public float BlockTime;
	public float CooldownTime;

	private float LastBlockTime;

	public void Start(){
		BlockTime = 3.0f;
		CooldownTime = 9.0f;
	}

	public void Block(){
		if (!gameObject.transform.FindChild ("BossShield").gameObject.activeInHierarchy) {
			if ((Time.time - LastBlockTime) > CooldownTime) {
				gameObject.transform.FindChild ("BossShield").gameObject.SetActive (true);
				LastBlockTime = Time.time;
			}
		}
	}
		
	// Update is called once per frame
	void Update () {
		if ((Time.time - LastBlockTime) > BlockTime) {
			gameObject.transform.FindChild ("BossShield").gameObject.SetActive (false);
		}
	}
}
