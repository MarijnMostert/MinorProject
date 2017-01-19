using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayText : MonoBehaviour {

	private Text BoldText;
	private Image BoldSpeechImage;
	public AudioClip boldTalkingClip;
	private GameManager gameManager;

	[Header ("if spider room:")]
	public bool SpiderRoom = false;
	public GameObject Spider;

	[TextArea(3,10)]
	public string myKeyboardText;
	[TextArea(3,10)]
	public string myControllerText;

	void Start () {
		gameManager = GameManager.Instance;
		Pet bold = gameManager.Bold.GetComponent<Pet> ();
		BoldSpeechImage = bold.speechImage;
		BoldText = bold.speechText;
	}
		
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			Debug.Log ("Triggered " + name);
			BoldSpeechImage.gameObject.SetActive (true);
			if (!gameManager.playerManagers [0].playerMovement.controllerInput) {
				BoldText.text = myKeyboardText;
			} else {
				BoldText.text = myControllerText;
			}
			AudioSource audio = GameManager.Instance.Bold.GetComponent<AudioSource> ();
			audio.clip = boldTalkingClip;
			audio.Play ();

			if (SpiderRoom) {
				Spider.SetActive (false);
				Spider.SetActive (true);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			BoldSpeechImage.gameObject.SetActive (false);
			BoldText.text = "";
		}
	}
}
