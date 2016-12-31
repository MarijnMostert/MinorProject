using UnityEngine;
using System.Collections;

public class Decoy : MonoBehaviour {

	public float activeTime;
	private GameObject originalTarget;
	private GameManager gameManager;

	void Start () {
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
		gameManager.enemyTarget = this.gameObject;
		Invoke ("Reset", activeTime);
	}

	void Reset(){
		Torch torch = gameManager.torch;
		if (torch.equipped) {
			gameManager.enemyTarget = torch.transform.parent.parent.gameObject;
		} else {
			gameManager.enemyTarget = torch.gameObject;
		}
		Destroy (gameObject);
	}

	void Update(){

	}
}
