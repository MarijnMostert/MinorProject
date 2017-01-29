using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.AI;

public class BossBehaviour : MonoBehaviour, IDamagable {

	private GameManager gameManager;
	public Animator anim;

	//Boss Attributes
	public float speed;
	public float deviation;
	public int startingHealth;
	public int health;
	private int scoreValue = 250;
	public bool dead;
	public string _name = "Aragog";

	public NavMeshAgent navMeshAgent;
	public Vector3 targetPosition;
	private WeaponController weapon;
	protected GameObject healthBar;
	protected Image healthBarImage;
	public GameObject target;
	public Projectile normalProjectile;

	private float lastSpecAtt;
	private float specCooldown;

	void Awake(){
		navMeshAgent = GetComponent<NavMeshAgent> ();
	}

	//Initialize Boss
	void Start () {
		gameManager = GameManager.Instance;
		dead = false;
		health = startingHealth + 100 * gameManager.dungeonLevel;
		lastSpecAtt = 0;
		specCooldown = 8;
		gameObject.transform.FindChild ("BossShield").gameObject.SetActive (false);
		target = GameObject.FindGameObjectWithTag("Player");
		weapon = GetComponent<WeaponController> ();
		weapon.currentWeapon.GetComponent<RangedWeapon> ().setProjectile (normalProjectile, 0.4f, 9);
		StartCoroutine (LookAtPlayer ());
		StartCoroutine (action ());
		StartCoroutine (UpdatePath ());
	}

	//Keep facing the player
	private IEnumerator LookAtPlayer () {
		while (dead != true) {
			transform.LookAt (new Vector3 (target.transform.position.x, target.transform.position.y, target.transform.position.z));
			yield return null;
		}
	}
		
	private IEnumerator UpdatePath(){
		while (!dead) {
			targetPosition = new Vector3 (transform.parent.position.x + Random.Range(-10, 10), 0, transform.parent.position.z + Random.Range(-10, 10));
			if (navMeshAgent.enabled) {
				navMeshAgent.SetDestination (targetPosition);
			} else {
				navMeshAgent.enabled = true;
			}
			yield return new WaitForSeconds (4);
		}
	}
//	private IEnumerator Walk() {
//		while (dead != true) {
//			float rand = Random.value;
//			if (rand > 0.5f && rand < 0.625f) {
//				transform.Translate (Vector3.left, speed * Time.deltaTime);
//				transform.Translate (Vector3.left);
//			}
//			if (rand > 0.625f && rand < 0.75f) {
//				transform.Translate (Vector3.right, speed * Time.deltaTime);
//			}
//			if (rand > 0.75f && rand < 0.875f) {
//				transform.Translate (Vector3.up, speed * Time.deltaTime);
//			}
//			if (rand > 0.875f) {
//				transform.Translate (Vector3.down, speed * Time.deltaTime);
//			}
//			yield return new WaitForSeconds (1);
//		}
//	}
		
	//Choose one ore more actions based on network output
	private IEnumerator action(){
		while (dead != true) {
			//Block
			if (Random.value > 0.7) {
				bool block = gameObject.GetComponent<BossBlock> ().Block ();
				if (block) {
					yield return new WaitForSeconds (5.0f);
				}
			}
			//Normal Attack
			if (Random.value > 0.04f) {
				anim.SetTrigger ("attack");
				Vector3 weaponRotOriginal = weapon.gameObject.transform.eulerAngles;
				Vector3 newWeaponRot = weaponRotOriginal + new Vector3 (0f, Random.Range (-deviation, deviation), 0f);
				weapon.gameObject.transform.eulerAngles = newWeaponRot;
				weapon.Fire ();
				weapon.gameObject.transform.eulerAngles = weaponRotOriginal;
				yield return null;
			}

			//special attack
			if (Random.value > 0.8f) {
				if (Time.time - lastSpecAtt > specCooldown) {
					StopAllCoroutines ();
					if (navMeshAgent.enabled) {
						navMeshAgent.SetDestination (transform.position);
					} 
					StartCoroutine (SpecialAttack ());
				}

			}
			yield return new WaitForSeconds (1f);;
		}
	}

	private IEnumerator SpecialAttack(){
		weapon.currentWeapon.GetComponent<RangedWeapon> ().setCooldown (0.01f);
		for (int i = 0; i < 36; i++) {
			transform.Rotate (new Vector3 (0, 10, 0));
			//transform.Rotate (Vector3.up, 300 * Time.deltaTime);
			weapon.Fire ();
			yield return new WaitForSeconds(0.05f);
		}
		lastSpecAtt = Time.time;
		weapon.currentWeapon.GetComponent<RangedWeapon> ().setCooldown (0.4f);
		StartCoroutine (action ());
		StartCoroutine (LookAtPlayer ());
		StartCoroutine (UpdatePath ());
	}
		
	private void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Torch")) {
			gameManager.torch.takeDamage(50, false, gameObject);
		}
	}

	//set inactive
	public void Die(){
		dead = true;
		gameManager.achievements.bossAchievement ();
		gameManager.updateScore (scoreValue);
		gameObject.GetComponentInParent<BossFight> ().ActivateLever ();
		healthBar.SetActive (false);
		Destroy (gameObject);
	}

	//For when the enemy object takes damage
	public void takeDamage(int damage, bool crit, GameObject source){
		if (healthBar == null) {
			InstantiateHealthBar ();
		}

		health -= damage;
		float lerp = (float)health / (float)startingHealth;
		healthBarImage.color = Color.Lerp(Color.red, Color.magenta, lerp);
		healthBarImage.fillAmount = lerp;
		//healthBar.fillAmount = (float)health / startingHealth;
		if (health <= 0)
			Die ();
	}

	void InstantiateHealthBar (){
		Vector3 healthBarPosition = transform.position + new Vector3 (0, 4, 0);
		healthBar = ObjectPooler.Instance.GetObject (20, true, healthBarPosition, transform);
		healthBarImage = healthBar.transform.Find ("HealthBar").GetComponent<Image> ();
		healthBar.transform.GetComponentInChildren<Text> ().text = _name;
		healthBar.transform.localScale.Scale(new Vector3(3, 3, 3));
	}
}