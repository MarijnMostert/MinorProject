using UnityEngine;
using System.Collections;

public class BossFight : AudioObject {

	public GameObject BossPrefab;
	private Vector3 spawnpoint;
	private GameObject BossObject;
	public GameObject[] torches;
	public AudioClip bossfightClip;
	AudioSource bossMusic;
	bool boss = false;
	private float tempVolume;

	// Use this for initialization
	void Start () {
		spawnpoint = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
	}

	void OnTriggerEnter(Collider other){
		//if (gameManager.dungeonLevel % 5 == 0) {
		if (boss == false) {
			if (other.gameObject.CompareTag ("Player")) {
				DeactivateLever ();
				Debug.Log ("boss instantiated");
				boss = true;
				BossObject = Instantiate (BossPrefab);
				BossObject.transform.position = spawnpoint;
				BossObject.transform.parent = gameObject.transform;
				for (int i = 0; i < torches.Length; i++) {
					torches [i].transform.GetChild (0).gameObject.SetActive (true);
					torches [i].transform.GetChild (1).gameObject.SetActive (true);
					torches [i].transform.GetChild (2).gameObject.SetActive (true);
					torches [i].transform.GetChild (3).gameObject.SetActive (true);
				}
				bossMusic = ObjectPooler.Instance.PlayAudioSource (bossfightClip, mixerGroup, pitchMin, pitchMax, transform);
				tempVolume = GameManager.Instance.audioSourceMusic.volume;
				GameManager.Instance.audioSourceMusic.volume = 0;
			}
		}
		//}
	}

	public void DeactivateLever(){
		gameObject.GetComponentInChildren<LeverActivator> ().Deactivate ();
	}

	public void ActivateLever(){
		bossMusic.Stop ();
		GameManager.Instance.audioSourceMusic.volume = tempVolume;
		gameObject.GetComponentInChildren<LeverActivator> (true).Activate ();
	}

}
