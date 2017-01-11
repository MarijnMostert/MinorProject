using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class Nest : AudioObject, IDamagable {
    public GameObject enemy;
    Spawner spawner;
    bool player;

    public int startingHealth;
    public int scoreValue = 100;
    public GameObject healthBarPrefab;
    [SerializeField]
    protected int health;
    protected GameObject healthBar;
    protected Image healthBarImage;
    protected GameManager gameManager;

    public bool dead;
    public AudioClip clip_takeDamage;
    public AudioClip clip_die;

    void Awake()
    {
        dead = false;
    }

    // Use this for initialization
    void Start () {
        health = startingHealth;
        player = false;
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        gameManager = spawner.gameManager;
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
        if (lerp <= .5f)
        {
            lerp *= 2f;
            healthBarImage.color = Color.Lerp(Color.red, Color.yellow, lerp);
        }
        else if (lerp > .5f)
        {
            lerp -= .5f;
            lerp *= 2f;
            healthBarImage.color = Color.Lerp(Color.yellow, Color.green, lerp);
        }
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
        healthBar = Instantiate(healthBarPrefab, healthBarPosition, transform.rotation, transform) as GameObject;
        healthBarImage = healthBar.transform.FindChild("HealthBar").GetComponent<Image>();
        healthBar.transform.localScale.Scale(new Vector3(3, 3, 3));
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = true;
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
        Destroy(healthBar);
        //Add a score
        gameManager.updateScore(scoreValue);
        StopAllCoroutines();
        Destroy(gameObject);
        yield return null;
    }

    IEnumerator spawn()
    {
        //yield return new WaitForSeconds (timeTillSpawning);
        while (player)
        {
            if (!spawner.dead && !gameManager.paused)
            {
                for (int i = 0; i < spawner.enemiesPerWave/2; i++)
                {
                    Instantiate(enemy, transform.position, Quaternion.identity);
                    yield return new WaitForSecondsRealtime(spawner.timeBetweenEnemySpawn/50);
                }
            }
            //if (player)
            //{
            //    yield return new WaitForSecondsRealtime(spawner.timeBetweenEnemySpawn);
            //} else
           // {
                yield return new WaitForSecondsRealtime(spawner.timeBetweenEnemySpawn/5);
           // }
        }
    }

}
