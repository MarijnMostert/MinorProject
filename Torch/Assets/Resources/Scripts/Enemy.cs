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
	[SerializeField]
	protected NavMeshAgent navMeshAgent;
	protected float lastAttackTime = 0f;
	protected GameObject healthBar;
	protected GameManager gameManager;

	protected virtual void Awake() {
		navMeshAgent = GetComponent<NavMeshAgent> ();
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager>();
	}

	protected virtual void Start () {
		health = startingHealth;
	}

	//Get the distance between the enemy and the torch
	protected float distanceToTarget(){
		if (gameManager.enemyTarget != null) {
			Vector3 distV3 = transform.position - gameManager.enemyTarget.transform.position;
			float distFl = Mathf.Abs (distV3.magnitude);
			return distFl;
		}
		return 0;
	}

	//For when the enemy object takes damage
	public void takeDamage(int damage){
		Debug.Log (gameObject + " takes " + damage + " damage.");

		if (healthBar == null) {
			InstantiateHealthBar ();
		}

		health -= damage;
		healthBar.transform.FindChild("HealthBar").GetComponent<Image>().fillAmount = (float)health / startingHealth;
		if (health <= 0)
			Die ();
	}

	void InstantiateHealthBar (){
		Vector3 healthBarPosition = transform.position + new Vector3 (0, 2, 0);
		healthBar = Instantiate (healthBarPrefab, healthBarPosition, transform.rotation, transform) as GameObject;
	}

	//When the enemy's health drops below 0.
	public void Die(){
		Debug.Log(gameObject + " died.");

		//Add a score
		gameManager.UpdateScore(scoreValue);
		Destroy (gameObject);
	}
}
