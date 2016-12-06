using UnityEngine;
using System.Collections;

public class MinimapController : MonoBehaviour {

	public GameObject player;
	public Vector3 offset;

	// Use this for initialization
	void Start () {
		transform.position = offset + player.transform.position;
	}

	// Update is called once per frame
	void LateUpdate () {
		transform.position = player.transform.position + offset;
	}
}
