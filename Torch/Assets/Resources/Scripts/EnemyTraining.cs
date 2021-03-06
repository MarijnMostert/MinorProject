using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyTraining : MonoBehaviour, IDamagable {

	public int startingHealth;
	public NavMesh navMesh;
	public float attackCooldown;
	public int attackDamage;
	public float refreshTime = 0.1f;
	public float attackRange = 1f;
	public int scoreValue = 10;

	[SerializeField]
	public int health;
	public GameObject target;
	protected NavMeshAgent navMeshAgent;
	protected float lastAttackTime = 0f;
	//protected ScoreManager scoreManager;
	protected bool dead;
	protected GameObject healthBar;

	protected virtual void Awake() {
		navMeshAgent = GetComponent<NavMeshAgent> ();
		//scormanager temporarily disabled
		//scoreManager = GameObject.Find("Score Manager").GetComponent<ScoreManager> ();
	}

	protected virtual void Start () {
		//Define the target.
		target = GameObject.FindGameObjectWithTag("Boss");
		dead = false;
		health = startingHealth;

	}

	//Get the distance between the enemy and the torch
	protected float distanceToTarget(){
		if (target != null) {
			Vector3 distV3 = transform.position - target.transform.position;
			float distFl = Mathf.Abs (distV3.magnitude);
			return distFl;
		}
		return 0;
	}

	//For when the enemy object takes damage
	public void takeDamage(int damage, bool crit, GameObject source){
		if (healthBar == null) {
			InstantiateHealthBar ();
		}

		health -= damage;
		healthBar.transform.FindChild("HealthBar").GetComponent<Image>().fillAmount = (float)health / startingHealth;

		if (health <= 0)
			Die ();
	}

	//When the enemy's health drops below 0.
	public void Die(){
		//Add a score
		//scoreManager.updateScore (scoreValue);
		dead = true;
		healthBar.SetActive (false);
		Destroy (gameObject);
	}

	void InstantiateHealthBar (){
		Vector3 healthBarPosition = transform.position + new Vector3 (0, 2, 0);
		healthBar = ObjectPooler.Instance.GetObject (14, true, healthBarPosition, transform.rotation, transform);
	}
}
