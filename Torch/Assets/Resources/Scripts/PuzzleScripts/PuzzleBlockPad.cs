using UnityEngine;
using System.Collections;

public class PuzzleBlockPad : MonoBehaviour {

	public string TriggerObjectName = "Block1";
	public bool activated = false;
	public Enemy[] enemiesToSpawn;
	private bool enemiesSpawned = false;
	private Vector3 smoothDampVar;
	private GameObject block;
	private bool notDone = true;
	public ParticleSystem[] particlesOnActivate;
	public ParticleSystem Indicator;

	void OnTriggerEnter(Collider other){
		if(other.gameObject.name.Equals(TriggerObjectName)){
			//Debug.Log(true);
			activated = true;
			block = other.gameObject;
			other.GetComponent<Rigidbody> ().isKinematic = true;

			/*Vector3 desiredPosition = transform.position;
			desiredPosition.y = other.transform.position.y;
			other.transform.position = Vector3.Lerp (other.transform.position, desiredPosition, .01f);
			Debug.Log ("Pad: " + transform.position + " Block: " + other.transform.position + " Desired Pos: " + desiredPosition);
			*/
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.name.Equals (TriggerObjectName)) {
			//Debug.Log (false);
			activated = false;
		}
	}

	void Update(){
		if (notDone && activated) {
			Vector3 desiredPosition = transform.position;
			desiredPosition.y = block.transform.position.y;
			block.transform.position = Vector3.SmoothDamp (block.transform.position, desiredPosition, ref smoothDampVar, .4f);
			if ((block.transform.position - desiredPosition).magnitude < 0.1f) {
				foreach (ParticleSystem PS in particlesOnActivate) {
					PS.Play ();
				}
				Indicator.Stop ();
				Destroy (block);
				foreach (Enemy enemy in enemiesToSpawn) {
					ObjectPooler.Instance.GetObject (enemy.ObjectPoolIndex, true, desiredPosition, Quaternion.Euler (new Vector3 (0, Random.Range (0, 360), 0)));
				}
				notDone = false;
			}
		}
	}

	/*
	IEnumerator StopBlock(GameObject GO){
		//GO.GetComponent<Rigidbody> ().isKinematic = true;
		Vector3 desiredPosition = transform.position;
		desiredPosition.y = GO.transform.position.y;
		Vector3.SmoothDamp (GO.transform.position, desiredPosition, ref smoothDampVar, 1f);
		yield return null;

	}
	*/
}
