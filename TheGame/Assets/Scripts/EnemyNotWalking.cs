using UnityEngine;
using System.Collections;

public class EnemyNotWalking : MonoBehaviour, IDamagable {

	public int health;
	public float attackCooldown;
	public int attackDamage;
	public float refreshTime = 0.1f;
	public float attackRange = 1f;
	public int scoreValue = 10;

	private GameObject target;
	private float lastAttackTime = 0f;
	private ScoreManager scoreManager;
	private bool dead;

	void Awake() {
		scoreManager = GameObject.Find("Score Manager").GetComponent<ScoreManager> ();
	}

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Torch");
		dead = false;
	}
	
	// Update is called once per frame
	void Update () {
			if (target != null) {
				doDamage ();
			}
	}

	private void doDamage(){
		float distance = distanceToTorch ();
		if(distance < attackRange && (Time.time - lastAttackTime) > attackCooldown){
			target.GetComponent<Torch> ().takeDamage (attackDamage);
			lastAttackTime = Time.time;
		}
	}

	//Get the distance between the enemy and the torch
	private float distanceToTorch(){
		Vector3 distV3 = transform.position - target.transform.position;
		float distFl = Mathf.Abs(distV3.magnitude);
		return distFl;
	}

	//For when the enemy object takes damage
	public void takeDamage(int damage){
		health -= damage;
		if (health <= 0)
			die ();
	}

	//When the enemy's health drops below 0.
	private void die(){
		//Add a score
		scoreManager.updateScore (scoreValue);
		dead = true;
		Destroy (gameObject);
	}

}
