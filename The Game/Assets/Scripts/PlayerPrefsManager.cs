using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour {

	public int controllerInput;
	public int highscore;

	// Use this for initialization
	void Start () {
		LoadAll ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.V)){
			SaveAll();
		}
	}

	void SaveAll(){
		PlayerPrefs.SetInt ("controllerInput", controllerInput);
		PlayerPrefs.SetInt ("highscore", highscore);
	}

	void LoadAll(){
		controllerInput = PlayerPrefs.GetInt ("controllerInput");
		highscore = PlayerPrefs.GetInt ("highscore");
	}
}
