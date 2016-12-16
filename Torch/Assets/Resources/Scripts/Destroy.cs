using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour {

	public float timeToDestroy;

	void Start () {
		Destroy (gameObject, timeToDestroy);
	}
}
