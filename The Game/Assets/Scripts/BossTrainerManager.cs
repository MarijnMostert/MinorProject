using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossTrainerManager : MonoBehaviour {

	/*
	static BossTrainerManager instantie;
	public GameObject bossPrefab;
	public GameObject gladiatorPrefab;
	public Transform SpawnPointBoss;
	public Transform SpawnPointGladiator;
	public Text UI;
	private int generationNumber;
	private int bossNumber;
	public int amountOfGenerations;

	void Start () {
		if (instantie != null) {
			GameObject.Destroy (gameObject);
		} else {
			GameObject.DontDestroyOnLoad (gameObject);
			instantie = this;
		}

		StartCoroutine (PopulationLoop ());
	}

	// This is called from start and will run each phase of the game one after another.
	private IEnumerator PopulationLoop ()
	{

		StartCoroutine (BossLoop ());

		generationNumber++;

		// This code is not run until 'RoundEnding' has finished.  At which point, check if a game winner has been found.
		if (generationNumber == amountOfGenerations){
			return;
		}
		else
		{
			// If there isn't a winner yet, restart this coroutine so the loop continues.
			// Note that this coroutine doesn't yield.  This means that the current version of the GameLoop will end.
			StartCoroutine (PopulationLoop ());
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

		// Increment the round number and display text showing the players what round it is.
		bossNumber++;
	}

	private IEnumerator RoundPlaying ()
	{
		// As soon as the round begins playing let the players control the tanks.
		EnableTankControl ();

		// Clear the text from the screen.
		m_MessageText.text = string.Empty;

		// While there is not one tank left...
		while (!OneTankLeft())
		{
			// ... return on the next frame.
			yield return null;
		}
	}


	private IEnumerator RoundEnding ()
	{
		// Stop tanks from moving.
		DisableTankControl ();

		// Clear the winner from the previous round.
		m_RoundWinner = null;

		// See if there is a winner now the round is over.
		m_RoundWinner = GetRoundWinner ();

		// If there is a winner, increment their score.
		if (m_RoundWinner != null)
			m_RoundWinner.m_Wins++;

		// Now the winner's score has been incremented, see if someone has one the game.
		m_GameWinner = GetGameWinner ();

		// Get a message based on the scores and whether or not there is a game winner and display it.
		string message = EndMessage ();
		m_MessageText.text = message;

		// Wait for the specified length of time until yielding control back to the game loop.
		yield return m_EndWait;
	}
*/
}