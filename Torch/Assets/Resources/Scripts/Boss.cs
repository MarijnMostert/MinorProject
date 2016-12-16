using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Boss : MonoBehaviour, IDamagable {

	//Neural Network Attributes
	public float[,] weights;
	public float[] input;
	private float[] threshold;
	public float[] actionThreshold;
	public List<GameObject> bulletsNearby = new List<GameObject>();
	public int inputNeurons = 8;
	public int outputNeurons = 7;
	private float[] rawOutput;
	public float[] finalOutput;
	private GameObject BulletA;
	private GameObject BulletB;

	//Genetic Algorithm Attributes
	public float timeAlive;
	public int damageDealt = 0;
	public int usedAttacks = 0;
	public int usedSpecAttacks = 0;
	public int usedBlocks = 0;
	public float timeAliveFactor;
	public float damageDealtFactor;
	public float ratioFactor;
	public float distMoved = 0;
	public float distMovedFactor;
	public float fitness;

	//Boss Attributes
	public Projectile normalProjectile;
	public Projectile specialProjectile;
	public GameObject target;
	public Vector3 oldPos;
	public float speed;
	public int startingHealth;
	public int health;
	public Color colorBoss;
	public int scoreValue = 1000;
	public GameObject healthBarPrefab;
	protected GameObject healthBar;
	protected ScoreManager scoreManager;
	public bool dead;

	//Initialize Boss and Neural Network weights
	void Start () {
		dead = false;
		timeAlive = Time.time;
		health = startingHealth;
		colorBoss = transform.GetComponent<MeshRenderer> ().material.color;
		gameObject.transform.FindChild ("BossShield").gameObject.SetActive (false);
		initialiseArraySizes ();
		//initialiseThresholds ();
		initialiseActionThresholds ();
		//initialiseWeights ();
		target = GameObject.FindGameObjectWithTag("Gladiator");
//		scoreManager = GameObject.Find("Score Manager").GetComponent<ScoreManager> ();
		StartCoroutine(DistanceMoved());
	}

	//Keep facing the player. Run the neural network.
	void Update () {
		transform.LookAt (new Vector3 (target.transform.position.x, target.transform.position.y, target.transform.position.z));
		colorBoss = Color.grey;
		selectInputs ();
		runNN ();
		action ();
	}

	public void initializeWeightsFromChromosome(float[] chromosome){
		Debug.Log ("initialize weights from chromosome");
		int counter = 0;
		weights = new float[inputNeurons, outputNeurons];
		for (int i = 0; i < inputNeurons; i++) {
			for (int j = 0; j < outputNeurons; j++) {
				weights [i, j] = chromosome [counter];
				counter++;
			}
		}
	}

	public void initializeThresholdsFromChromosome(float[] chromosome){
		Debug.Log ("initialize thresholds from chromosome");
		int counter = inputNeurons * outputNeurons;
		threshold = new float[outputNeurons];
		for (int i = 0; i < outputNeurons; i++) {
				threshold [i] = chromosome [counter];
				counter++;
		}
	}



	//Randomly initializes the network tresholds
	void initialiseThresholds ()
	{
		threshold = new float[outputNeurons];
		for (int i = 0; i < threshold.Length; i++) {
			threshold [i] = Random.value;
		}
	}

	//Randomly initializes action tresholds
	void initialiseActionThresholds(){
		actionThreshold = new float[outputNeurons];
		//Action tresholds for moving
		for (int i = 0; i < 4; i++){
			actionThreshold [i] = 0.5f;
		}
		//Action tresholds for attacking and blocking
		for (int i = 4; i < actionThreshold.Length; i++) {
			actionThreshold [i] = 0.8f;
		}
	}

	//Randomly initializes network weights
	void initialiseWeights () {
		weights = new float[inputNeurons, outputNeurons];
		for (int i = 0; i < inputNeurons; i++) {
			for (int j = 0; j < outputNeurons; j++) {
				weights [i, j] = Random.value;
			}
		}
	}

	//initializes the arrays that will hold environment information and network output
	void initialiseArraySizes(){
		input = new float[inputNeurons];
		rawOutput = new float[outputNeurons];
		finalOutput = new float[outputNeurons];
	}

	//Sense the environment and store normalized and capped data
	void selectInputs () {
		//normalized player position
		input [0] = normalize(target.transform.position.x - transform.position.x, 40f);
		input [1] = normalize(target.transform.position.z - transform.position.z, 40f);

		//cap player position (-1, 1)
		if (input [0] > 1f)
			input [0] = 1f;
		else if (input [0] < -1f)
			input [0] = -1f;

		if (input [1] > 1f)
			input [1] = 1f;
		else if (input [1] < -1f)
			input [1] = -1f;

		//Two closest bullets
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

		//Distance to the center of the room
		input [6] = normalize (Mathf.Abs(transform.position.z), 25);
		input [7] = normalize (Mathf.Abs(transform.position.x), 25);
	}

	//normalize an input to a given value
	float normalize(float value, float max){
		return value / max;
	}

	//Finds all bullets currently in scene and pick the two closest
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

	//Find the distance between Boss and another object
	float getDist(GameObject other){
		return (other.transform.position - gameObject.transform.position).magnitude;
	}


	//find the distance moved by the boss
	private IEnumerator DistanceMoved(){
		oldPos = transform.position;
		while (health > 0) {
			distMoved += (oldPos - transform.position).magnitude;
			oldPos = transform.position;
			yield return new WaitForSeconds (1f);
		}
	}


	//Runs the Neural network
	void runNN(){
		for (int i = 0; i < outputNeurons; i++) {
			rawOutput[i] = 0;
			for (int j = 0; j < inputNeurons; j++) {
				rawOutput [i] += input [j] * weights [j,i];
			}
			finalOutput [i] = sigmoid (rawOutput [i] - threshold[i]);
		}
	}

	//Returns the sigmoid of a float
	float sigmoid(float x){
		return 1 / (1 + Mathf.Exp (-x));
	}

	//Choose one ore more actions based on network output
	void action(){
		//move Left
		if (finalOutput [0] > actionThreshold[0]) {
			transform.position = transform.position + speed * Time.deltaTime * new Vector3 (-1f, 0f, 0f);
		}
		//move right
		if (finalOutput [1] > actionThreshold[1]) {
			transform.position = transform.position + speed * Time.deltaTime * new Vector3 (1f, 0f, 0f);
		}
		//Move Up
		if (finalOutput [2] > actionThreshold[2]) {
			transform.position = transform.position + speed * Time.deltaTime * new Vector3 (0f, 0f, 1f);
		}
		//Move Down
		if (finalOutput [3] > actionThreshold[3]) {
			transform.position = transform.position + speed * Time.deltaTime * new Vector3 (0f, 0f, -1f);
		}
		//Normal Attack
		if (finalOutput [4] > actionThreshold[4]) {
			transform.GetComponent<MeshRenderer>().material.color = Color.red;
			GetComponent<WeaponController> ().currentWeapon.GetComponent<RangedWeapon> ().setProjectile (normalProjectile, 0.3f, 9);
			GetComponent<WeaponController> ().Fire ();
			usedAttacks++;
		}
		//Block
		else if (finalOutput [5] > actionThreshold[5]) {
			gameObject.GetComponent<BossBlock>().Block();
			usedBlocks++;
		}
		//Special Attack
		else if (finalOutput [6] > actionThreshold[6]) {
			transform.GetComponent<MeshRenderer>().material.color = Color.blue;
			GetComponent<WeaponController> ().currentWeapon.GetComponent<RangedWeapon> ().setProjectile (specialProjectile, 1.0f, 30);
			GetComponent<WeaponController> ().Fire ();
			usedSpecAttacks++;
		}

	}

	//Calculate boss fitness and set inactive
	public void Die(){
		CalculateFitness ();
//		scoreManager.updateScore (scoreValue);
		dead = true;
		Destroy (healthBar.transform.parent.gameObject);
		Destroy (gameObject);
	}

	public void CalculateFitness(){
		//calculate ratio between normal and special attacks. ideal is 5:1.
		float ratio = CalculateRatio();
		float diffFromIdealRatio = 5.0f - ratio;

		//Find the amound of damage done to the gladiator
		damageDealt = target.GetComponent<EnemyTraining>().startingHealth - target.GetComponent<EnemyTraining>().health;

		//time alive
		timeAlive = Time.time - timeAlive;

		//Actual calculation
		fitness = distMoved * distMovedFactor + timeAlive * timeAliveFactor + damageDealt * damageDealtFactor;// - diffFromIdealRatio * ratioFactor;
		Debug.Log ("dist: " + distMoved);
		Debug.Log ("time: " + timeAlive);
		Debug.Log ("damage: " + damageDealt);
		//Debug.Log ("diff ratio: " + diffFromIdealRatio);

		//If no attacks, no blocks or no special attacks have been used, fitness is halved
		if (usedAttacks == 0 || usedSpecAttacks == 0 || usedBlocks == 0) {
			fitness = fitness/2;
		}

		//cap negative fitness to 0
		if (fitness < 0){
			fitness = 0;
		}

		//send fitness to the Trainermanager
		GameObject.Find ("Ground").GetComponent<TrainerManager> ().TemporaryFitness = fitness;
	}

	public float CalculateRatio(){
		if(usedAttacks != 0){
			return usedSpecAttacks / usedAttacks;
		}
		return usedSpecAttacks / (usedAttacks + 1);
	}

	//For when the enemy object takes damage
	public void takeDamage(int damage){
		if (healthBar == null) {
			InstantiateHealthBar ();
		}
			
		health -= damage;
		healthBar.transform.FindChild("HealthBar").GetComponent<Image>().fillAmount = (float)health / startingHealth;
		//healthBar.fillAmount = (float)health / startingHealth;
		if (health <= 0)
			Die ();
	}

	void InstantiateHealthBar (){
		Vector3 healthBarPosition = transform.position + new Vector3 (0, 2, 0);
		healthBar = Instantiate (healthBarPrefab, healthBarPosition, transform.rotation, transform) as GameObject;
	}
}