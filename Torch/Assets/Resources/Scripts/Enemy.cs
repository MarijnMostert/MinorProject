using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour, IDamagable {

	public int startingHealth;
	public NavMesh navMesh;
	public float attackCooldown;
	public int attackDamage;
	public float refreshTime = 0.3f;
	public float attackRange = 1f;
	public int scoreValue = 10;
	public GameObject healthBarPrefab;
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
	protected AudioSource audioSource;


	protected virtual void Awake() {
		navMeshAgent = GetComponent<NavMeshAgent> ();
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager>();

		dead = false;

		audioSource = GetComponent<AudioSource> ();

	}

	protected virtual void Start () {
		health = startingHealth;
		speed = gameObject.GetComponent<NavMeshAgent> ().speed;
        dead = false;

		if (clip_spawn != null) {
			audioSource.clip = clip_spawn;
			audioSource.pitch = Random.Range (0.9f, 1.1f);
			audioSource.Play ();
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
	public void takeDamage(int damage, bool crit){
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
			audioSource.clip = clip_takeDamage;
			audioSource.pitch = Random.Range (0.9f, 1.1f);
			audioSource.Play ();
		}

		if (health <= 0) {
			if (anim != null)
			{
				Debug.Log ("animation time");
				anim.SetTrigger ("Die");
			}
			Debug.Log ("dead");
			Die ();
		}
	}

	void InstantiateHealthBar (){
		Vector3 healthBarPosition = transform.position + new Vector3 (0, 2, 0);
		healthBar = Instantiate (healthBarPrefab, healthBarPosition, transform.rotation, transform) as GameObject;
		healthBarImage = healthBar.transform.FindChild ("HealthBar").GetComponent<Image> ();
	}

    public void Die()
    {
		if (clip_die != null) {
			audioSource.clip = clip_die;
			audioSource.pitch = Random.Range (0.9f, 1.1f);
			audioSource.Play ();
		}
        StartCoroutine(DieThread());
    }

	//When the enemy's health drops below 0.
	private IEnumerator DieThread(){
        //Debug.Log(gameObject + " died.");
        dead = true;
//        Debug.Log(anim);
        if (anim != null)
        {
            yield return new WaitForSeconds(1.25f);//.56f
        }
        //Add a score
        gameManager.updateScore(scoreValue);
        StopAllCoroutines();
        Destroy(gameObject);
        yield return null;

	}

    public void setAnim(Animator animator)
    {
        anim = animator;
    }
}
