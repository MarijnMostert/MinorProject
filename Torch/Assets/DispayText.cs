using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UI;

public class DispayText : MonoBehaviour {

	public bool keyboard;

	[TextArea(3,10)]
	public string myKeyboardText;

	[TextArea(3,10)]
	public string myControllerText;

	GameObject myUI;

	// Use this for initialization
		void Start () {
		myUI = GameObject.Find ("TutText");
		myUI.GetComponent<Text> ().text = "";

	}

	void OnTriggerEnter () {
		Debug.Log ("Entered");
		if (keyboard) {
			myUI.GetComponent<Text> ().text =  myKeyboardText;
		} else {
			myUI.GetComponent<Text> ().text =  myControllerText;
		}
	}

	void OnTriggerExit() {
		myUI.GetComponent<Text> ().text =  "";
	}
}
