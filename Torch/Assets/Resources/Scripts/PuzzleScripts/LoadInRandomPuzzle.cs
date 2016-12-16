using UnityEngine;
using System.Collections;

public class LoadInRandomPuzzle : MonoBehaviour {

	public Vector3 Center;
	public GameObject[] Prefabs;

	// Use this for initialization
	void Start () {
		int no = Random.Range (0, Prefabs.Length);
		GameObject chosen = Prefabs [no];
		Instantiate (chosen, Center, Quaternion.identity);
	}
}
