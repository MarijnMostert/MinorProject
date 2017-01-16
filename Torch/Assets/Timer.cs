using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour {

	private GameManager gameManager;
	private float startingTime;
	public Text timerText;

	// Use this for initialization
	void Start () {
		startingTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Equals)) {
			Time.timeScale += .5f;
		}
		if (Input.GetKeyDown (KeyCode.Minus)) {
			Time.timeScale -= .5f;
		}

		timerText.text = "Time: " + timeToString (Time.time - startingTime);
	}

	/// <summary>
	/// Takes in time and returns a string in format (M)M:SS
	/// </summary>
	/// <returns>time in string format</returns>
	/// <param name="pastTime">The amount of time that needs to be converted</param>
	static string timeToString(float pastTime){
		string minutesString = ((int)(pastTime / 60)).ToString();
		string secondsString = "";
		int seconds = (int)(pastTime % 60);
		if (seconds < 10) {
			secondsString = "0" + seconds.ToString ();
		} else {
			secondsString = seconds.ToString ();
		}

		return minutesString + ":" + secondsString;
	}

	public void Reset(){
		startingTime = Time.time;
	}
}
