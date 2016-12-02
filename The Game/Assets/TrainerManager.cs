using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TrainerManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (Boss ());
	}

	private IEnumerator Boss(){
		//while boss is alive
		while (GameObject.FindGameObjectWithTag ("Boss") != null) {
			Debug.Log ("loop");
			// ... return on the next frame.
			yield return null;
		}
		Debug.Log("load scene");
		SceneManager.LoadScene ("Boss Fight");
	}
}
