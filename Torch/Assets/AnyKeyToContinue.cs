using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AnyKeyToContinue : MonoBehaviour {

	private bool boolean = false;

	void Update(){
		if (Input.anyKeyDown && !boolean) {
			SceneManager.LoadScene ("create_dungeon");
			boolean = true;
		}
	}
}
