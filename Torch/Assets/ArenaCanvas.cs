using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArenaCanvas : MonoBehaviour {

	public Text waveStarted;
	public Text waveCleared;

	public void Start(){
		waveStarted.gameObject.SetActive (false);
		waveCleared.gameObject.SetActive (false);
	}

	public void WaveStarted(int waveNumber){
		waveStarted.text = "Wave " + waveNumber + " has begun";
		waveStarted.gameObject.SetActive (true);
	}

	public void WaveCleared(int waveNumber){
		waveCleared.text = "Wave " + waveNumber + " is cleared";
		waveCleared.gameObject.SetActive (true);
	}
}
