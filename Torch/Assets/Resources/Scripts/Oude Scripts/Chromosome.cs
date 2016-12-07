using UnityEngine;
using System.Collections;

public class Chromosome : MonoBehaviour {

	public float[] chromosome;

	public void constructor(int chromSize){
		chromosome = new float[chromSize];
		initializeRandom();
	}

	void initializeRandom(){
		for (int i = 0; i < chromosome.Length; i++) {
			chromosome [i] = Random.value;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
