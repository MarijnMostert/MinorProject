using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManager1 : MonoBehaviour {

	static SceneManager1 Instance;
	public static Scene scene1, scene2, scene3, scene4;

	// Use this for initialization
	void Start () {
		if (Instance != null) {
			GameObject.Destroy (gameObject);
		} else {
			GameObject.DontDestroyOnLoad (gameObject);
			Instance = this;
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Alpha0)){
			SceneManager.LoadScene (0);
		}
		if (Input.GetKeyUp (KeyCode.Alpha1)) {
			SceneManager.LoadScene (1);
		}
	}
}