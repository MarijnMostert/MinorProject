using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Boss : MonoBehaviour, IDamagable {

	private GameManager gameManager;
	public Animator anim;

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

	//Boss Attributes
	public Projectile normalProjectile;
	public Projectile specialProjectile;
	public GameObject target;
	public float speed;
	public int startingHealth;
	public int health;
	public int scoreValue = 1000;
	public GameObject healthBarPrefab;
	protected GameObject healthBar;
	public bool dead;


	//Initialize Boss and Neural Network weights
	void Start () {
		gameManager = GameManager.Instance;
		dead = false;
		health = startingHealth;
		gameObject.transform.FindChild ("BossShield").gameObject.SetActive (false);
		initialiseArraySizes ();
		initialiseActionThresholds ();
		initializeBestWeightsAndTresholds();
		target = GameObject.FindGameObjectWithTag("Player");
	}

	//Keep facing the player. Run the neural network.
	void Update () {
		transform.LookAt (new Vector3 (target.transform.position.x, target.transform.position.y, target.transform.position.z));
		selectInputs ();
		runNN ();
		action ();
		gameObject.GetComponent<BossBlock>().Block();
	}
		
	public void initializeBestWeightsAndTresholds(){
		weights = new float[inputNeurons, outputNeurons];
		threshold = new float[outputNeurons];
		float[] temp = new float[]{0.5678023f, 0.5750628f, 0.4790469f, 0.5926962f, 0.3444433f, 0.3393099f, 0.9480648f, 0.01853831f, 0.7728073f, 0.6357021f, 0.9657403f, 0.2877342f, 0.3078995f, 0.2167819f, 0.5540775f, 0.9004681f, 0.6841441f, 0.5982641f, 0.9076774f, 0.7365416f, 0.8793281f, 0.5822603f, 0.1047627f, 0.7878622f, 1.005986f, 0.6845614f, 0.06608278f, 0.1070118f,-0.06569222f, 0.7428343f, 0.5287511f, 0.8968663f, 0.4001851f, 0.8392711f, 0.7895258f, 0.00608548f, 0.05639189f, 0.8475735f, 0.2506087f, 0.6174976f, 0.1670547f, 0.5621039f, 0.297895f, 0.2158358f, 0.9671163f, 0.8106396f, 0.3786803f, 0.03473509f, 0.5133829f, 0.5102599f, 0.340109f, 0.2945383f, 0.420847f, 0.9740732f, 0.06681152f, 0.8949148f, 0.02159697f, 0.8221027f, 0.4287531f, 0.366612f, 0.4611591f, 0.6392899f, 0.5363703f}; 
		int counter = 0;
		for (int i = 0; i < inputNeurons; i++) {
			for (int j = 0; j < outputNeurons; j++) {
				weights [i, j] = temp [counter];
				counter++;
			}
		}
		for (int i = 0; i < outputNeurons; i++) {
			threshold [i] = temp [counter];
			counter++;
		}
	}
		
	//initializes action tresholds
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
		
	//initializes the arrays that will hold environment information and network output
	void initialiseArraySizes(){
		input = new float[inputNeurons];
		rawOutput = new float[outputNeurons];
		finalOutput = new float[outputNeurons];
	}

	//Sense the environment and store normalized and capped data
	void selectInputs () {
		//normalized player position
		input [0] = normalize(target.transform.position.x - transform.position.x, 15f);
		input [1] = normalize(target.transform.position.z - transform.position.z, 15f);

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
		input [6] = normalize (Mathf.Abs(transform.position.z), 16);
		input [7] = normalize (Mathf.Abs(transform.position.x), 16);
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
			if (transform.position.magnitude < 12.0f) {
				transform.position = transform.position + speed * Time.deltaTime * new Vector3 (-1f, 0f, 0f);
			}
		}
		//move right
		if (finalOutput [1] > actionThreshold[1]) {
			if (transform.position.magnitude < 12.0f) {
				transform.position = transform.position + speed * Time.deltaTime * new Vector3 (1f, 0f, 0f);
			}
		}
		//Move Up
		if (finalOutput [2] > actionThreshold[2]) {
			if (transform.position.magnitude < 12.0f) {
				transform.position = transform.position + speed * Time.deltaTime * new Vector3 (0f, 0f, 1f);
			}
		}
		//Move Down
		if (finalOutput [3] > actionThreshold[3]) {
			if (transform.position.magnitude < 12.0f) {
				transform.position = transform.position + speed * Time.deltaTime * new Vector3 (0f, 0f, -1f);
			}
		}
		//Normal Attack
		if (finalOutput [4] > actionThreshold[4]) {
			anim.SetTrigger ("attack");
			GetComponent<WeaponController> ().currentWeapon.GetComponent<RangedWeapon> ().setProjectile (normalProjectile, 0.5f, 9);
			GetComponent<WeaponController> ().Fire ();
		}
		//Block
		else if (finalOutput [5] > actionThreshold[5]) {
			//gameObject.GetComponent<BossBlock>().Block();
		}
		//Special Attack
		else if (finalOutput [6] > actionThreshold[6]) {
			anim.SetTrigger ("specAttack");
			GetComponent<WeaponController> ().currentWeapon.GetComponent<RangedWeapon> ().setProjectile (specialProjectile, 2.0f, 25);
			GetComponent<WeaponController> ().Fire ();
		}

	}

	//set inactive
	public void Die(){
		dead = true;
		gameManager.updateScore (scoreValue);
		gameObject.GetComponentInParent<BossFight> ().ActivateLever ();
		Destroy (healthBar.transform.parent.gameObject);
		Destroy (gameObject);
	}

	//For when the enemy object takes damage
	public void takeDamage(int damage, bool crit){
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