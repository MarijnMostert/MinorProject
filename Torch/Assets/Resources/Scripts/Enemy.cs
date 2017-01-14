using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class Enemy : AudioObject, IDamagable {

	public int ObjectPoolIndex;
	public int startingHealth;
	public NavMesh navMesh;
	public float attackCooldown;
	public int attackDamage;
	public float refreshTime = 0.3f;
	public float attackRange = 1f;
	public int scoreValue = 10;
	[HideInInspector] public float speed;

	[SerializeField] protected int health;
	[SerializeField] protected NavMeshAgent navMeshAgent;
	protected float lastAttackTime = 0f;
	protected GameObject healthBar;
	protected Image healthBarImage;
	protected GameManager gameManager;

    public Animator anim;
    public bool dead;
	public AudioClip clip_takeDamage;
	public AudioClip clip_attack;
	public AudioClip clip_spawn;
	public AudioClip clip_die;
	public AudioClip clip_battleCry;

	protected bool firstTimeActive = true;

	public int getHealth () {
		return health;
	}

	protected virtual void Awake() {
		navMeshAgent = GetComponent<NavMeshAgent> ();
		gameManager = GameManager.Instance;
	}

	protected virtual void OnEnable(){			
		
		Reset ();
		if (clip_spawn != null) {
			ObjectPooler.Instance.PlayAudioSource (clip_spawn, mixerGroup, pitchMin, pitchMax, transform);
		}
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
	public void takeDamage(int damage, bool crit, GameObject source){
		//Debug.Log (gameObject + " takes " + damage + " damage.");
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
			ObjectPooler.Instance.PlayAudioSource (clip_takeDamage, mixerGroup, pitchMin, pitchMax, transform);
		}

		if (health <= 0) {
			if (anim != null)
			{
//				Debug.Log ("animation time");
				anim.SetTrigger ("Die");
//				Debug.Log (anim.GetCurrentAnimatorClipInfo (0).Length);
			}
//			Debug.Log ("dead");

			ReturnPlayerData (source);
			Die ();
		}
	}

	void InstantiateHealthBar (){
		Vector3 healthBarPosition = transform.position + new Vector3 (0, 2, 0);
		healthBar = ObjectPooler.Instance.GetObject (14, true, healthBarPosition, transform.rotation, transform);
		healthBarImage = healthBar.transform.FindChild ("HealthBar").GetComponent<Image> ();
		healthBar.transform.localScale.Scale(new Vector3(3, 3, 3));
	}

    public void Die()
    {
		if (clip_die != null) {
			ObjectPooler.Instance.PlayAudioSource (clip_die, mixerGroup, pitchMin, pitchMax, transform);
		}
		Drop ();
        StartCoroutine(DieThread());
    }

	//When the enemy's health drops below 0.
	private IEnumerator DieThread(){
        //Debug.Log(gameObject + " died.");
        dead = true;
		GetComponent<Collider> ().enabled = false;
		navMeshAgent.enabled = false;
		healthBar.SetActive (false);
		healthBar = null;
//        Debug.Log(anim);
        if (anim != null)
        {
			yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length - 0.02f);//.56f
        }
        //Add a score
        gameManager.updateScore(scoreValue);
        StopAllCoroutines();
		gameObject.SetActive (false);
        yield return null;

	}

    public void setAnim(Animator animator)
    {
        anim = animator;
    }

	public void Reset(){
		GetComponent<Collider> ().enabled = true;
		navMeshAgent.enabled = true;
		dead = false;
		health = startingHealth;
		speed = navMeshAgent.speed;
	}

	public void Drop(){
		float rand = Random.value;
		if (rand > 0.5){
			ObjectPooler.Instance.GetObject (18, true, new Vector3(transform.position.x, 1f, transform.position.z));
		}
		if (rand > 0.9) {
			ObjectPooler.Instance.GetObject (17, true, new Vector3(transform.position.x, 1f, transform.position.z));
		}
	}

	public void ReturnPlayerData(GameObject source){
		Projectile projectile = source.GetComponent<Projectile> ();
		PlayerData playerData = null;
		if (projectile != null) {
			playerData = projectile.PlayerData;
		} else {
			LaserWeapon laserWeapon = source.GetComponent<LaserWeapon> ();
			if (laserWeapon != null) {
				playerData = laserWeapon.playerData;
			}
		}

		if (playerData != null) {
			playerData.IncrementEnemiesKilled ();
		}
	}
}
