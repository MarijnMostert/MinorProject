using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss : MonoBehaviour {

	public float[,] weights;
	public float[] input;
	public float[] threshold;
	public float[] actionThreshold;
	public GameObject target;
	public List<GameObject> bulletsNearby = new List<GameObject>();
	public int inputNeurons = 6;
	public int outputNeurons = 7;
	public float[] rawOutput;
	public float[] finalOutput;
	public GameObject BulletA;
	public GameObject BulletB;
	public float speed;


	void Start () {
		initialiseArraySizes ();
		initialiseThresholds ();
		initialiseActionThresholds ();
		initialiseWeights ();
		target = GameObject.Find ("Player");
	}
	
	void Update () {
		gameObject.GetComponent<MeshRenderer> ().material.color = Color.grey;
		selectInputs ();
		runNN ();
		action ();
	}

	void initialiseThresholds ()
	{
		threshold = new float[outputNeurons];
		for (int i = 0; i < threshold.Length; i++) {
			threshold [i] = Random.value;
		}
	}

	void initialiseActionThresholds(){
		actionThreshold = new float[outputNeurons];
		for (int i = 0; i < actionThreshold.Length; i++){
			actionThreshold [i] = 0.6f;
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
		input [0] = normalize(target.transform.position.x - transform.position.x, 40f);
		input [1] = normalize(target.transform.position.z - transform.position.z, 40f);

		if (input [0] > 1f)
			input [0] = 1f;
		else if (input [0] < -1f)
			input [0] = -1f;
		
		if (input [1] > 1f)
			input [1] = 1f;
		else if (input [1] < -1f)
			input [1] = -1f;
		
		findClosestBullets ();
		if (BulletA != null) {
			input [2] = normalize(BulletA.transform.position.x - transform.position.x, 100f);
			input [3] = normalize(BulletA.transform.position.z - transform.position.z, 100f);
		} else {
			input [2] = 1f;
			input [3] = 1f;
		}

		if (BulletB != null) {
			input [4] = normalize(BulletB.transform.position.x - transform.position.x, 100);
			input [5] = normalize(BulletB.transform.position.z - transform.position.z, 100);
		} else {
			input [4] = 1f;
			input [5] = 1f;
		}
	}

	float normalize(float value, float max){
		return value / max;
	}

	void findClosestBullets(){
		if (bulletsNearby != null) {
			for (int j = 0; j < bulletsNearby.Count; j++) {
				if (bulletsNearby [j] == null) {
					bulletsNearby.Remove(bulletsNearby[j]);
				}
			}

			GameObject[] bulletsNearbyA = bulletsNearby.ToArray();
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
					for (int i = 2; i < bulletsNearbyA.Length; i++) {
						if (bulletsNearbyA [i] != null) {
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
	}

	float getDist(GameObject other){
		return (other.transform.position - gameObject.transform.position).magnitude;
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag("Bullet")) {
			Debug.Log ("Bullet Enter");
			bulletsNearby.Add (other.gameObject);
		}
	}


	void OnTriggerStay(Collider other){
		if(other.gameObject.CompareTag("Bullet") && !bulletsNearby.Contains(other.gameObject)){
			Debug.Log ("Bullet stay");
			bulletsNearby.Add (other.gameObject);
		}
	}


	void OnTriggerExit(Collider other){
		if (other.gameObject.CompareTag("Bullet")) {
			Debug.Log ("Bullet exit");
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

	void action(){
		if (finalOutput [0] > actionThreshold[0]) {
			transform.Translate(speed * Time.deltaTime * new Vector3(-1f, 0f, 0f)); //left
		}

		if (finalOutput [1] > actionThreshold[1]) {
			transform.Translate(speed * Time.deltaTime * new Vector3(1f, 0f, 0f)); //right
		}

		if (finalOutput [2] > actionThreshold[2]) {
			transform.Translate(speed * Time.deltaTime * new Vector3(0f, 0f, 1f)); //up
		}

		if (finalOutput [3] > actionThreshold[3]) {
			transform.Translate(speed * Time.deltaTime * new Vector3(0f, 0f, -1f)); //down
		}

		if (finalOutput [4] > actionThreshold[4]) {
			//attack
			gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
		}

		if (finalOutput [5] > actionThreshold[5]) {
			//block
			gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
		}

		if (finalOutput [6] > actionThreshold[6]) {
			//special attack
			gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
		}

	}
}