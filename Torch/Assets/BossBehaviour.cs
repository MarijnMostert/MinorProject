using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BossBehaviour : MonoBehaviour, IDamagable {

	private GameManager gameManager;
	public Animator anim;

	//Boss Attributes
	public Projectile normalProjectile;
	public GameObject target;
	public float speed;
	public float deviation;
	public int startingHealth;
	public int health;
	private int scoreValue = 250;
	protected GameObject healthBar;
	protected Image healthBarImage;
	public bool dead;
	private WeaponController weapon;
	public string name = "aragog";

	private float lastSpecAtt;
	private float specCooldown;

	//Initialize Boss
	void Start () {
		gameManager = GameManager.Instance;
		dead = false;
		health = startingHealth;
		lastSpecAtt = 0;
		specCooldown = 8;
		gameObject.transform.FindChild ("BossShield").gameObject.SetActive (false);
		target = GameObject.FindGameObjectWithTag("Player");
		weapon = GetComponent<WeaponController> ();
		weapon.currentWeapon.GetComponent<RangedWeapon> ().setProjectile (normalProjectile, 0.4f, 9);
		StartCoroutine (LookAtPlayer ());
		StartCoroutine (action ());
	}

	//Keep facing the player
	private IEnumerator LookAtPlayer () {
		while (dead != true) {
			transform.LookAt (new Vector3 (target.transform.position.x, target.transform.position.y, target.transform.position.z));
			yield return null;
		}
	}
		
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
					StartCoroutine (SpecialAttack ());
				}
			}
			yield return new WaitForSeconds (1f);;
		}
	}

	private IEnumerator SpecialAttack(){
		weapon.currentWeapon.GetComponent<RangedWeapon> ().setCooldown (0.01f);
		Transform start = transform;
		for (int i = 0; i < 35; i++) {
			transform.Rotate (Vector3.up, 300 * Time.deltaTime);
			weapon.Fire ();
			yield return null;
		}
		lastSpecAtt = Time.time;
		weapon.currentWeapon.GetComponent<RangedWeapon> ().setCooldown (0.4f);
		StartCoroutine (action ());
		StartCoroutine (LookAtPlayer ());
	}
		
	private void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Torch")) {
			gameManager.torch.takeDamage(50, false, gameObject);
		}
	}

	//set inactive
	public void Die(){
		dead = true;
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
		healthBarImage.fillAmount = (float)health / startingHealth;
		//healthBar.fillAmount = (float)health / startingHealth;
		if (health <= 0)
			Die ();
	}

	void InstantiateHealthBar (){
		Vector3 healthBarPosition = transform.position + new Vector3 (0, 4, 0);
		healthBar = ObjectPooler.Instance.GetObject (20, true, healthBarPosition, transform);
		healthBarImage = healthBar.transform.Find ("HealthBar").GetComponent<Image> ();
		healthBar.transform.GetComponentInChildren<Text> ().text = name;
		healthBar.transform.localScale.Scale(new Vector3(3, 3, 3));
	}
}