using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UI;

public class DispayText : MonoBehaviour {

	private Text BoldText;


	public bool keyboard;

	[TextArea(3,10)]
	public string myKeyboardText;

	[TextArea(3,10)]
	public string myControllerText;

	GameObject myUI;

	// Use this for initialization
		void Start () {
		BoldText = GameObject.Find ("Game Manager").GetComponent<GameManager> ().Bold.GetComponentInChildren<Text> ();
//		myUI = GameObject.Find ("TutText");
//		myUI.GetComponent<Text> ().text = "";

	}

	void OnTriggerEnter () {
		Debug.Log ("Entered");
		if (keyboard) {
			BoldText.text =  myKeyboardText;
		} else {
			BoldText.text =  myControllerText;
		}
	}

	void OnTriggerExit() {
		BoldText.text =  "";
	}
}
