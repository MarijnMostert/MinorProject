using UnityEngine;
using System.Collections;

public class BossBlock : MonoBehaviour {

	private float BlockTime;
	private float CooldownTime;

	private float LastBlockTime;

	public void Start(){
		BlockTime = 5.0f;
		CooldownTime = 9.0f;
	}

	public bool Block(){
		if (!gameObject.transform.FindChild ("BossShield").gameObject.activeInHierarchy) {
			if ((Time.time - LastBlockTime) > CooldownTime) {
				gameObject.transform.FindChild ("BossShield").gameObject.SetActive (true);
				LastBlockTime = Time.time;
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}
		
	// Update is called once per frame
	void Update () {
		if ((Time.time - LastBlockTime) > BlockTime) {
			gameObject.transform.FindChild ("BossShield").gameObject.SetActive (false);
		}
	}
}
