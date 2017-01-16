using UnityEngine;
using System.Collections;

public class MiniMapSettings : MonoBehaviour {

	GameManager myManager;
	Camera myCamera;

	// Use this for initialization
	void Start () {
		myManager = GameManager.Instance;
		myCamera = GetComponent<Camera> ();

		myCamera.transform.position = new Vector3((float)myManager.dungeonData.dungeonParameters[myManager.dungeonLevel].width - 2.5f, 33.0f,
			(float)myManager.dungeonData.dungeonParameters[myManager.dungeonLevel].height - 2.5f) * 6 / 2;
		myCamera.orthographicSize = myManager.dungeonData.dungeonParameters[myManager.dungeonLevel].height * 6 / 2;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
