using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArenaCanvas : MonoBehaviour {

	public Text waveStarted;
	public Text waveCleared;
	public Text nextArea;

	public void Start(){
		waveStarted.gameObject.SetActive (false);
		waveCleared.gameObject.SetActive (false);
		nextArea.gameObject.SetActive (false);
	}

	public void WaveStarted(int waveNumber){
		waveStarted.text = "Wave " + waveNumber + " has begun";
		waveStarted.gameObject.SetActive (true);
	}

	public void WaveCleared(int waveNumber){
		waveCleared.text = "Wave " + waveNumber + " is cleared";
		waveCleared.gameObject.SetActive (true);
	}
	public void NextArea(string AreaName){
		nextArea.text = "Go To " + AreaName;
		nextArea.gameObject.SetActive (true);
	}
}
