using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour, IDamagable {

	public int startingHealth;
	public NavMesh navMesh;
	public float attackCooldown;
	public int attackDamage;
	public float refreshTime = 0.1f;
	public float attackRange = 1f;
	public int scoreValue = 10;
	public GameObject healthBarPrefab;

	[SerializeField]
	protected int health;
	protected GameObject target;
	protected NavMeshAgent navMeshAgent;
	protected float lastAttackTime = 0f;
	protected ScoreManager scoreManager;
	protected bool dead;
	protected Image healthBar;

	protected virtual void Awake() {
		navMeshAgent = GetComponent<NavMeshAgent> ();
		scoreManager = GameObject.Find("Score Manager").GetComponent<ScoreManager> ();
	}

	protected virtual void Start () {
		//Define the target.
		target = GameObject.FindGameObjectWithTag ("Torch").transform.parent.gameObject;
		dead = false;
		health = startingHealth;
		Vector3 healthBarPosition = transform.position + new Vector3 (0, 2, 0);
		GameObject obj = Instantiate (healthBarPrefab, healthBarPosition, transform.rotation) as GameObject;
		healthBar = obj.transform.FindChild ("HealthBar").GetComponent<Image> ();
		obj.GetComponent<Follow> ().target = gameObject;
		healthBar.fillAmount = 1f;
	}

	//Get the distance between the enemy and the torch
	protected float distanceToTarget(){
		Vector3 distV3 = transform.position - target.transform.position;
		float distFl = Mathf.Abs(distV3.magnitude);
		return distFl;
	}

	//For when the enemy object takes damage
	public void takeDamage(int damage){
		health -= damage;
		healthBar.fillAmount = (float)health / startingHealth;
		if (health <= 0)
			die ();
	}

	//When the enemy's health drops below 0.
	private void die(){
		//Add a score
		scoreManager.updateScore (scoreValue);
		dead = true;
		Destroy (healthBar.transform.parent.gameObject);
		Destroy (gameObject);
	}
}
