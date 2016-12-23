using UnityEngine;
using System.Collections;

public class MiniMapSettings : MonoBehaviour {

	GameManager myManager;
	Camera myCamera;

	// Use this for initialization
	void Start () {
		myManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
		myCamera = GetComponent<Camera> ();

		myCamera.transform.position = new Vector3((float)myManager.width - 2.5f, 33.0f, (float)myManager.height - 2.5f) * 6 / 2;
		myCamera.orthographicSize = myManager.height * 6 / 2;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
