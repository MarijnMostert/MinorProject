using UnityEngine;
using System.Collections;

public class PickupKey : MonoBehaviour {

	GameObject bridges;
	public bool troes = false;
	public GameObject myParent;

	void Start() {
		troes = true;
		bridges = (GameObject) Resources.Load ("Prefabs/Bridges", typeof(GameObject));
		GameObject instantiated = Instantiate (bridges);
		instantiated.transform.parent = myParent.transform;

		OpenDoors ();
	}
		
	void OpenDoors() {}

}
