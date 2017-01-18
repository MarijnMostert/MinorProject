using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class Nest : AudioObject {
    public Enemy enemy;
    Spawner spawner;
    bool player;

    public int startingHealth;
    public int scoreValue = 100;
    [SerializeField]
    protected int health;
    protected GameObject healthBar;
    protected Image healthBarImage;
	public string name;
    protected GameManager gameManager;

    public bool dead;
    public AudioClip clip_takeDamage;
    public AudioClip clip_die;

    public GameObject spawnpoint;
    public int numberOfEnemies;

    void Awake()
    {
        dead = false;
    }

    // Use this for initialization
    void Start () {
        health = startingHealth;
        player = false;
		gameManager = GameManager.Instance;
		spawner = gameManager.spawner;
	}

    //For when the enemy object takes damage
	public void takeDamage(int damage, bool crit, GameObject source)
    {
        //Debug.Log (gameObject + " takes " + damage + " damage.");

        if (healthBar == null)
        {
            InstantiateHealthBar();
        }

        health -= damage;
        healthBarImage.fillAmount = (float)health / startingHealth;
        float lerp = (float)health / (float)startingHealth;
     
		healthBarImage.color = Color.Lerp(Color.red, Color.magenta, lerp);
        
        DamagePopUp.CreateDamagePopUp(damage, gameObject, crit);

        if (clip_takeDamage != null)
        {
			ObjectPooler.Instance.PlayAudioSource (clip_takeDamage, mixerGroup, pitchMin, pitchMax, transform);
        }

        if (health <= 0)
        {
            //			Debug.Log ("dead");
            StartCoroutine(spawn());
            Die();
        }
        if (Random.value < .3f)
        {
            StartCoroutine(spawn());
        }
    }

    void InstantiateHealthBar()
    {
        Vector3 healthBarPosition = transform.position + new Vector3(0, 2, 0);
		healthBar = ObjectPooler.Instance.GetObject (20, true, healthBarPosition, transform);
        healthBarImage = healthBar.transform.FindChild("HealthBar").GetComponent<Image>();
		healthBar.transform.GetComponentInChildren<Text> ().text = name;
        healthBar.transform.localScale.Scale(new Vector3(3, 3, 3));
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = true;
            Debug.Log("spawn triggered");
            StartCoroutine(spawn());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = false;
        }
    }

    public void Die()
    {
        if (clip_die != null)
        {
			ObjectPooler.Instance.PlayAudioSource (clip_die, mixerGroup, pitchMin, pitchMax, transform);
        }
        StartCoroutine(DieThread());
    }

    //When the enemy's health drops below 0.
    private IEnumerator DieThread()
    {
        //Debug.Log(gameObject + " died.");
        dead = true;
		healthBar.SetActive (false);
		//Add a score
        gameManager.updateScore(scoreValue);
        StopAllCoroutines();
		gameObject.SetActive (false);
        yield return null;
    }

    IEnumerator spawn()
    {
        //yield return new WaitForSeconds (timeTillSpawning);
        while (player)
        {
            if (!spawner.dead && !gameManager.paused)
            {
                for (int i = 0; i < numberOfEnemies; i++)
                {
					ObjectPooler.Instance.GetObject (enemy.ObjectPoolIndex, true, spawnpoint.gameObject.transform.position, 
						Quaternion.Euler (new Vector3 (0f, Random.Range (0, 360), 0f)));
                    yield return new WaitForSecondsRealtime(spawner.timeBetweenEnemySpawn/50);
                }
            }
            //if (player)
            //{
            //    yield return new WaitForSecondsRealtime(spawner.timeBetweenEnemySpawn);
            //} else
           // {
                yield return new WaitForSecondsRealtime(spawner.timeBetweenEnemySpawn);
           // }
        }
    }

}
