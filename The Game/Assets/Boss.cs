using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

	public float[,] weights;
	public float[] input;
	public float[] threshold;
	public GameObject target;
	public ArrayList bulletsNearby;
	public int inputNeurons = 6;
	public int outputNeurons = 7;
	public float[] rawOutput;
	public float[] finalOutput;
	public GameObject BulletA;
	public GameObject BulletB;


	// Use this for initialization
	void Start () {
		initialiseArraySizes ();
		initialiseThresholds ();
		initialiseWeights ();
		target = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		selectInputs ();
		runNN ();
	}

	void initialiseThresholds ()
	{
		threshold = new float[outputNeurons];
		for (int i = 0; i < threshold.Length; i++) {
			threshold [i] = Random.value;
		}
	}

	void initialiseWeights ()
	{
		weights = new float[inputNeurons, outputNeurons];
		for (int i = 0; i < inputNeurons; i++) {
			for (int j = 0; j < outputNeurons; j++) {
				weights [i, j] = Random.value;
			}
		}
	}

	void initialiseArraySizes(){
		input = new float[inputNeurons];
		/*
		for (int i = 0; i < input.Length; i++) {
			input [i] = Random.value;
		}
		*/

		rawOutput = new float[outputNeurons];
		finalOutput = new float[outputNeurons];
	}

	void selectInputs () {
		input [0] = target.transform.position.x - transform.position.x;
		input [1] = target.transform.position.y - transform.position.y;
		findClosestBullets ();
		if (BulletA != null) {
			input [2] = BulletA.transform.position.x - transform.position.x;
			input [3] = BulletA.transform.position.y - transform.position.y;
		} else {
			input [2] = 100f;
			input [3] = 100f;
		}

		if (BulletB != null) {
			input [4] = BulletB.transform.position.x - transform.position.x;
			input [5] = BulletB.transform.position.y - transform.position.y;
		} else {
			input [4] = 100f;
			input [5] = 100f;
		}
	}

	void findClosestBullets(){
		if (bulletsNearby != null) {
			GameObject[] bulletsNearbyA = bulletsNearby.ToArray () as GameObject[];
			if (bulletsNearbyA.Length == 0) {
				BulletA = null;
				BulletB = null;
			} else if (bulletsNearbyA.Length == 1) {
				BulletA = bulletsNearbyA [0];
				BulletB = null;
			} else {
				BulletA = bulletsNearbyA [0];
				BulletB = bulletsNearbyA [1];
				if (bulletsNearbyA.Length > 2) {
					for (int i = 0; i < bulletsNearbyA.Length; i++) {
						if (getDist (bulletsNearbyA [i]) < getDist (BulletA)) {
							BulletA = bulletsNearbyA [i];
						} else {
							if (getDist (bulletsNearbyA [i]) < getDist (BulletB)) {
								BulletB = bulletsNearbyA [i];
							}
						}
					}
				}
			}
		}
	}

	float getDist(GameObject other){
		return (other.transform.position - gameObject.transform.position).magnitude;
	}

	void OnTriggerEnter(Collider other){
		Debug.Log (other.gameObject);
		if (other.gameObject.CompareTag("Bullet")) {
			bulletsNearby.Add (other.gameObject);
		}
	}

	void OnTriggerStay(Collider other){
		if(other.gameObject.CompareTag("Bullet") && !bulletsNearby.Contains(other.gameObject)){
			Debug.Log (other.gameObject);
			bulletsNearby.Add (other.gameObject);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.CompareTag("Bullet")) {
			Debug.Log (other.gameObject);
			bulletsNearby.Remove (other.gameObject);
		}
	}

	void runNN(){
		for (int i = 0; i < outputNeurons; i++) {
			rawOutput[i] = 0;
			for (int j = 0; j < inputNeurons; j++) {
				rawOutput [i] += input [j] * weights [j,i];
			}
			finalOutput [i] = sigmoid (rawOutput [i] - threshold[i]);
		}
	}
	float sigmoid(float x){
		return 1 / (1 + Mathf.Exp (-x));
	}
}