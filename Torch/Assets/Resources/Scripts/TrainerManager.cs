using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TrainerManager : MonoBehaviour {

	public GA genAlg;
	public GameObject bossPrefab;
	public Transform SpawnPointBoss;
	public GameObject gladiatorPrefab;
	public Transform SpawnPointGladiator;
	public int BossCounter;
	public float TemporaryFitness = 0;

	// Use this for initialization
	void Start () {
		genAlg = new GA (20, 63);
		genAlg.CreatePopulation ();
		BossCounter = 1;
		Debug.Log ("First Boss");
		Instantiate(gladiatorPrefab, SpawnPointGladiator.position, SpawnPointGladiator.rotation);
		Instantiate(bossPrefab, SpawnPointBoss.position, SpawnPointBoss.rotation);
		GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss> ().initializeWeightsFromChromosome (genAlg.getChrom(0));
		GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss> ().initializeThresholdsFromChromosome (genAlg.getChrom(0));
		StartCoroutine (Boss ());
	}



	private IEnumerator Boss(){
		while (BossCounter < 20) {
			//while boss is alive
			while (GameObject.FindGameObjectWithTag ("Boss") != null) {
				
				// ... return on the next frame.
				yield return null;
			}
			genAlg.fitness [BossCounter - 1] = TemporaryFitness;
			Debug.Log ("Boss " + BossCounter + ", Fitness: " + genAlg.fitness [BossCounter - 1]);

			Debug.Log ("KILL GLADIATOR");
			DestroyImmediate (GameObject.FindGameObjectWithTag ("Gladiator"));

			Debug.Log ("ff wachten...");
			yield return new WaitForSeconds (4f);

			BossCounter++;
			Debug.Log ("Start boss " + BossCounter);
			Instantiate (gladiatorPrefab, SpawnPointGladiator.position, SpawnPointGladiator.rotation);
			Instantiate (bossPrefab, SpawnPointBoss.position, SpawnPointBoss.rotation);
			GameObject.FindGameObjectWithTag ("Boss").GetComponent<Boss> ().initializeWeightsFromChromosome (genAlg.getChrom (BossCounter - 1));
			GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss> ().initializeThresholdsFromChromosome (genAlg.getChrom(BossCounter - 1));
		}
		genAlg.fitness [BossCounter - 1] = TemporaryFitness;
		Debug.Log ("Boss " + BossCounter + ", Fitness: " + genAlg.fitness [BossCounter - 1]);
		Debug.Log ("Generation complete");

	}
}
