using UnityEngine;
using System.Collections;

public class BillboardMovie : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// this line of code will make the Movie Texture begin playing
		MovieTexture movie = ((MovieTexture)GetComponent<Renderer>().material.mainTexture);
		movie.Play();
		movie.loop = true;


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
