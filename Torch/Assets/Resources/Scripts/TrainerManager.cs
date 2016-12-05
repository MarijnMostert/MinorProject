using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TrainerManager : MonoBehaviour {

	public GA genAlg;
	public GameObject bossPrefab;
	public Transform SpawnPointBoss;
	public GameObject gladiatorPrefab;
	public Transform SpawnPointGladiator;

	// Use this for initialization
	void Start () {
		genAlg = new GA (20, 63);
		genAlg.CreatePopulation ();
		Instantiate(gladiatorPrefab, SpawnPointGladiator.position, SpawnPointGladiator.rotation);
		Instantiate(bossPrefab, SpawnPointBoss.position, SpawnPointBoss.rotation);
		GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss> ().initializeWeightsFromChromosome (genAlg.getChrom(0));
		StartCoroutine (Boss ());
	}



	private IEnumerator Boss(){
		//while boss is alive
		while (GameObject.FindGameObjectWithTag ("Boss") != null) {
			// ... return on the next frame.
			yield return null;
		}
		DestroyImmediate(GameObject.FindGameObjectWithTag("Gladiator"));
		Debug.Log("load scene");
		//SceneManager.LoadScene ("Boss Fight");
		yield return new WaitForSeconds(1f);
		Instantiate(gladiatorPrefab, SpawnPointGladiator.position, SpawnPointGladiator.rotation);
		Instantiate(bossPrefab, SpawnPointBoss.position, SpawnPointBoss.rotation);
	}
}
