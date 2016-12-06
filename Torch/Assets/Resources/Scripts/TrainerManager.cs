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
		genAlg = new GA (5, 63);
		genAlg.CreatePopulation ();
		StartCoroutine(Population (10));
	}

	private IEnumerator Population(int generations){
		for (int i = 0; i < generations; i++) {
			Debug.Log ("Starting generation " + (i+1));
			BossCounter = 0;
			yield return StartCoroutine (Boss ());
		}
	}

	private IEnumerator Boss(){
		while (BossCounter < (genAlg.sizePopulation)) {

			BossCounter++;
			Debug.Log ("Start boss " + BossCounter);
			Instantiate (gladiatorPrefab, SpawnPointGladiator.position, SpawnPointGladiator.rotation);
			Instantiate (bossPrefab, SpawnPointBoss.position, SpawnPointBoss.rotation);
			GameObject.FindGameObjectWithTag ("Boss").GetComponent<Boss> ().initializeWeightsFromChromosome (genAlg.getChrom (BossCounter - 1));
			GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss> ().initializeThresholdsFromChromosome (genAlg.getChrom(BossCounter - 1));

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
	}
}
