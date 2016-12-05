using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossTrainerManager : MonoBehaviour {


	static BossTrainerManager instance;
	public GameObject bossPrefab;
	public GameObject gladiatorPrefab;
	public Transform SpawnPointBoss;
	public Transform SpawnPointGladiator;
	public Text UI;
	private int generationNumber;
	private int bossNumber;
	public int amountOfGenerations;

	void Start () {
		if (instance != null) {
			GameObject.Destroy (gameObject);
		} else {
			GameObject.DontDestroyOnLoad (gameObject);
			instance = this;
		}

		StartCoroutine (PopulationLoop ());
	}



	// This is called from start and will run each population one after another
	private IEnumerator PopulationLoop ()
	{
		bossNumber = 0;
		StartCoroutine (BossLoop ());

		generationNumber++;
	
		if (generationNumber == amountOfGenerations){
			return null;
		}
		else
		{
			StartCoroutine (PopulationLoop ());
			return null;
		}
	}

	private IEnumerator BossLoop(){
		// Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
		yield return StartCoroutine (RoundStarting ());

		// Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
		yield return StartCoroutine (RoundPlaying());

		// Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
		yield return StartCoroutine (RoundEnding());
	}

	private IEnumerator RoundStarting ()
	{
		//Spawn boss and spawn enemy
		Instantiate(bossPrefab, SpawnPointBoss.position, SpawnPointBoss.rotation);
		Instantiate (gladiatorPrefab, SpawnPointGladiator.position, SpawnPointGladiator.rotation);

		UI.text = "Generation: " + generationNumber + " | bossNumber: " + bossNumber;

		bossNumber++;
		yield return null;
	}

	private IEnumerator RoundPlaying ()
	{

		// While boss is alive.
		while (!GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().dead)
		{
			// ... return on the next frame.
			yield return null;
		}
	}


	private IEnumerator RoundEnding ()
	{
		//Kill gladiator
		Destroy(GameObject.FindGameObjectWithTag("Enemy"));
	
		yield return null;
	}

}