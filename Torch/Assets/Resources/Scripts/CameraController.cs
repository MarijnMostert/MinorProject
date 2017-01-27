using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

	public Vector3 offset;
	public Vector3 rotation;
	public float smoothTime = 0.1f;
	public float smoothTimeTransition = 1f;

	public int importanceFactorP1;
	public int importanceFactorP2;
	public int importanceFactorTorch;

	private Vector3 targetPosition;
	private Vector3 smoothDampVelocity;
	private Vector3 cameraPosition;
	public GameManager gameManager;
	private List<GameObject> targets;
	private bool FreeCam = false;
	private Vector3 smoothDampVar;
	private Vector3 PuzzleTargetPos;
	public Vector3 PuzzleOffset;
	private bool transitionToPuzzle = false;
	private bool transitionFromPuzzle = false;
	private float timer;
	private string Mode;

	public float fpsSmoothingTime = .2f;
	private Transform fpsTarget;
	private Quaternion originalRotation;

	void Awake(){
		originalRotation = transform.rotation;
	}

	void Start(){
		gameManager = GameManager.Instance;
		targets = new List<GameObject> ();
		FreeCam = false;
		transitionFromPuzzle = false;
		transitionToPuzzle = false;
		transform.eulerAngles = rotation;
		Mode = "default";
	}

	void FixedUpdate () {
		//targetPosition = getAveragePosition ();

		if (!gameManager.FirstPerson) {
			switch (Mode) {
			case "Normal":
				targetPosition = getAveragePosition ();
				cameraPosition = targetPosition + offset;

				transform.position = Vector3.SmoothDamp (transform.position, cameraPosition, ref smoothDampVelocity, smoothTime);
				break;
			case "Puzzle":
				targetPosition = getAveragePosition ();
				targetPosition = (targetPosition + (PuzzleTargetPos * 3)) * .25f;
				cameraPosition = targetPosition + PuzzleOffset;

				transform.position = Vector3.SmoothDamp (transform.position, cameraPosition, ref smoothDampVar, smoothTime);
				break;
			case "TransitionToPuzzle":
				if (Time.time - timer < (smoothTimeTransition + .6f)) {
					targetPosition = getAveragePosition ();
					targetPosition = (targetPosition + (PuzzleTargetPos * 3)) * .25f;
					cameraPosition = targetPosition + PuzzleOffset;

					transform.position = Vector3.SmoothDamp (transform.position, cameraPosition, ref smoothDampVar, smoothTimeTransition);
				} else {
					Mode = "Puzzle";
				}
				break;
			case "TransitionFromPuzzle":
				if (Time.time - timer < (smoothTimeTransition + .6f)) {
					targetPosition = getAveragePosition ();
					cameraPosition = targetPosition + offset;

					transform.position = Vector3.SmoothDamp (transform.position, cameraPosition, ref smoothDampVelocity, smoothTimeTransition);
				} else {
					Mode = "Normal";
				}
				break;
			case "default":
				break;
			}
		} else {
			if (fpsTarget == null) {
				fpsTarget = gameManager.playerManagers [0].playerMovement.fpCamPosition;
			}
			transform.position = Vector3.SmoothDamp (transform.position, fpsTarget.position, ref smoothDampVelocity, fpsSmoothingTime);
			transform.rotation = fpsTarget.rotation;
		}
	

		/*
		if (gameManager.camTarget != null) {
			if (!transitionToPuzzle && !transtionFromPuzzle && !FreeCam) {
				//targetPosition = gameManager.camTarget.transform.position;
				targetPosition = getAveragePosition ();
				cameraPosition = targetPosition + offset;

				//The smoothdamp makes sure the camera follows the target(s) smoothly
				transform.position = Vector3.SmoothDamp (transform.position, cameraPosition, ref smoothDampVelocity, smoothTime);
				transform.eulerAngles = rotation;
			} else if (!transition) {
				targetPosition = getAveragePosition ();
				targetPosition = (targetPosition + (PuzzleTargetPos * 3)) * .25f;
				cameraPosition = targetPosition + PuzzleOffset;

				transform.position = Vector3.SmoothDamp (transform.position, cameraPosition, ref smoothDampVar, .25f);
			} else {
				targetPosition = getAveragePosition
			}
		}
		*/
	}
		
	public void ActivatePuzzleCam(Transform puzzleTransform){
		timer = Time.time;
		Mode = "TransitionToPuzzle";
		PuzzleTargetPos = puzzleTransform.position;
	}

	public void DeactivatePuzzleCam(){
		timer = Time.time;
		Mode = "TransitionFromPuzzle";
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
		if (GameManager.Instance.torch.torchPickUp.equipped) {
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

	public void SetMode(string Mode){
		this.Mode = Mode;
	}

	public void toggleFirstPerson(bool firstPerson){
		if (firstPerson) {
			transform.localPosition = new Vector3 (0, 2f, -2f);
			transform.LookAt (transform.position + Vector3.forward);
			Cursor.lockState = CursorLockMode.Locked;
		} else {
			transform.rotation = originalRotation;
			Cursor.lockState = CursorLockMode.None;
		}
	}
}
