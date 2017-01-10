using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AnyKeyToContinue : MonoBehaviour {

	void Update(){
		if (Input.anyKeyDown) {
			gameObject.SetActive (false);
		}
	}
}
