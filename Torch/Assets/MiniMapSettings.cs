using UnityEngine;
using System.Collections;

public class MiniMapSettings : MonoBehaviour {

	GameManager myManager;
	Camera myCamera;

	// Use this for initialization
	void Start () {
		myManager = GameManager.Instance;
		myCamera = GetComponent<Camera> ();

		int dungeonlevel = myManager.dungeonLevel;
		if (dungeonlevel > 40)
			dungeonlevel = 40;

		myCamera.transform.position = new Vector3((float)myManager.dungeonData.dungeonParameters[dungeonlevel].width - 2.5f, 33.0f,
			(float)myManager.dungeonData.dungeonParameters[dungeonlevel].height - 2.5f) * 6 / 2;
		myCamera.orthographicSize = myManager.dungeonData.dungeonParameters[dungeonlevel].height * 6 / 2;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
