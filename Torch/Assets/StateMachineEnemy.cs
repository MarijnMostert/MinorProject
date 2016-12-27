using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StateMachineEnemy : MonoBehaviour, IDamagable {

	//State machine properties
	public float refreshTime = 0.3f;
	public State state;
	public enum State {
		SPAWN,
		IDLE,
		CHASE,
		ATTACK,
		DIE
	}
	public bool alive = true;

	//Properties
	public int startingHealth;
	private int health;
	public float speed;
	public float sight;
	public float attackRange;
	private bool dead = false;
	private bool targetInRange = false;

	//References
	private GameManager gameManager;
	private NavMeshAgent navMeshAgent;
	private Animator anim;

	//Healthbar
	public GameObject healthBarPrefab;
	private GameObject healthBar;
	protected Image healthBarImage;

	//Audio
	public AudioClip clip_takeDamage;
	public AudioClip clip_attack;
	public AudioClip clip_spawn;
	public AudioClip clip_die;
	public AudioClip clip_battleCry;
	protected AudioSource audioSource;

	//To set the references
	void Awake(){
		navMeshAgent = GetComponent<NavMeshAgent> ();
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
		audioSource = GetComponent<AudioSource> ();
		anim = GetComponent<Animator> ();
	}

	void Start () {
		health = startingHealth;
		navMeshAgent.speed = 0f;
		state = State.IDLE;
		StartCoroutine (StateMachine ());
	}

	IEnumerator StateMachine(){
		while (alive) {
			switch (state) {
			case State.SPAWN:
				Spawn ();
				break;
			case State.IDLE:
				Idle ();
				break;
			case State.CHASE:
				Chase ();
				break;
			case State.ATTACK:
				Attack ();
				break;
			case State.DIE:
				Die ();
				break;
			}
			yield return new WaitForSeconds(refreshTime);
		}
	}

	public void takeDamage(int damage, bool crit){
		if (healthBar == null) {
			InstantiateHealthBar ();
		}

		health -= damage;
		healthBarImage.fillAmount = (float)health / startingHealth;
		float lerp = (float)health / (float)startingHealth;
		if (lerp <= .5f) {
			lerp *= 2f;
			healthBarImage.color = Color.Lerp (Color.red, Color.yellow, lerp);
		} else if (lerp > .5f) {
			lerp -= .5f;
			lerp *= 2f;
			healthBarImage.color = Color.Lerp (Color.yellow, Color.green, lerp);
		}
		DamagePopUp.CreateDamagePopUp(damage, gameObject, crit);




		if (clip_takeDamage != null) {
			audioSource.clip = clip_takeDamage;
			audioSource.pitch = Random.Range (0.9f, 1.1f);
			audioSource.Play ();
		}

		if (health <= 0) {
			if (anim != null)
			{
				anim.SetTrigger ("Die");
			}
			Die ();
		}
	}

	void InstantiateHealthBar (){
		Vector3 healthBarPosition = transform.position + new Vector3 (0, 2, 0);
		healthBar = Instantiate (healthBarPrefab, healthBarPosition, transform.rotation, transform) as GameObject;
		healthBarImage = healthBar.transform.FindChild ("HealthBar").GetComponent<Image> ();
		healthBar.transform.localScale.Scale(new Vector3(3, 3, 3));
	}

	void Spawn(){
		
	}

	void Idle(){
		//play idle animation
		if (targetInRange) {
			state = State.CHASE;
			navMeshAgent.speed = speed;
			navMeshAgent.SetDestination (gameManager.enemyTarget.transform.position);
			Debug.Log ("Now chasing");
		}
	}

	void Chase(){
		if (IsCloseEnoughForAttack ()) {
			state = State.ATTACK;
			Debug.Log ("Now attacking");
		} else if (!targetInRange) {
			state = State.IDLE;
			navMeshAgent.speed = 0f;
			Debug.Log ("Now idle");
		}
		navMeshAgent.SetDestination (gameManager.enemyTarget.transform.position);
		//play walking animation
		//move towards target
	}

	void Attack(){
		state = State.CHASE;
		Debug.Log ("Now chasing");
		//play attacking animation
		//attack
	}

	public void Die(){
		//play dying animation
		//disable damagable -> disable collider?
		//after that, destroy this enemy
		dead = true;
		Destroy(gameObject, 0.5f);
	}

	bool IsCloseEnoughForChase(){
		if (Mathf.Abs ((gameManager.enemyTarget.transform.position - transform.position).magnitude) < sight) {
			return true;
		} else {
			return false;
		}
	}

	bool IsCloseEnoughForAttack(){
		if (Mathf.Abs ((gameManager.enemyTarget.transform.position - transform.position).magnitude) < attackRange) {
			return true;
		} else {
			return false;
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			targetInRange = true;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.CompareTag ("Player")) {
			targetInRange = false;
		}
	}

}
