using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

	public GameObject target;
	public Vector3 offset;
	public Vector3 rotation;
	public float smoothTime = 0.1f;

	public int importanceFactorP1;
	public int importanceFactorP2;
	public int importanceFactorTorch;

	private Vector3 targetPosition;
	private Vector3 smoothDampVelocity;
	private Vector3 cameraPosition;
	public GameManager gameManager;
	private List<GameObject> targets;

	void Start(){
		gameManager = GameManager.Instance;
		targets = new List<GameObject> ();
	}

	void FixedUpdate () {
		//targetPosition = getAveragePosition ();
		if (gameManager.camTarget != null) {
			//targetPosition = gameManager.camTarget.transform.position;
			targetPosition = getAveragePosition();
			cameraPosition = targetPosition + offset;

			//The smoothdamp makes sure the camera follows the target(s) smoothly
			transform.position = Vector3.SmoothDamp (transform.position, cameraPosition, ref smoothDampVelocity, smoothTime);
			transform.eulerAngles = rotation;
		}
	}
		

	//Calculate the average position between all targets (players).
	private Vector3 getAveragePosition(){
		UpdateTargets ();
		Vector3 temp = new Vector3(0,0,0);
		for (int i = 0; i < targets.Count; i++) {
			if (targets [i] != null) {
				temp += targets [i].transform.position;
			}
		}
		temp = temp / targets.Count;
		return temp;
	}

	public void UpdateTargets(){
		targets = new List<GameObject> ();
		foreach (PlayerManager PM in gameManager.playerManagers) {
			//If this player is active
			if (PM.active) {
				int importanceFactor = 0;
				if (PM.playerNumber == 1) {
					importanceFactor = importanceFactorP1;
				} else if (PM.playerNumber == 2) {
					importanceFactor = importanceFactorP2;
				}

				for (int i = 0; i < importanceFactor; i++) {
					targets.Add (PM.playerInstance);
				}
			}
		}

		//Add torch to the list of targets 3 times. (or player holding the torch).
		if (gameManager.torch.torchPickUp.equipped) {
			for (int i = 0; i < importanceFactorTorch; i++) {
				if(gameManager.torch != null && gameManager.torch.transform.parent.parent.gameObject != null)
					targets.Add (gameManager.torch.transform.parent.parent.gameObject);
			}
		} else {
			for (int i = 0; i < importanceFactorTorch; i++) {
				if (gameManager.torch.gameObject != null) {
					targets.Add (gameManager.torch.gameObject);
				}
			}
		}
	}
}
