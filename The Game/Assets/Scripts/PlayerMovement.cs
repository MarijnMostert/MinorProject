using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public int playerNumber;
	public float speed;
	public LayerMask floorMask;
	public GameObject cursorPointer;
	public bool controllerInput;

	private string HorizontalAxis;
	private string VerticalAxis;
	private float HorizontalInput;
	private float VerticalInput;
	private float cameraRayLength = 200f;
	private RaycastHit floorHit;
	private Ray cameraRay;

	public PlayerPrefsManager playerPrefsManager;

	private string ControllerTurningHorizontalAxis;
	private string ControllerTurningVerticalAxis;
	private float ControllerTurningHorizontalInput;
	private float ControllerTurningVerticalInput;
	private string ControllerMovingHorizontalAxis;
	private string ControllerMovingVerticalAxis;
	private float ControllerMovingHorizontalInput;
	private float ControllerMovingVerticalInput;

	void Awake(){
		playerPrefsManager = GameObject.Find ("SceneManager").GetComponent<PlayerPrefsManager> ();
	}

	void Start () {
		//The input may differ for another player (e.g. arrow keys vs. wasd keys)
		HorizontalAxis = "Horizontal" + playerNumber;
		VerticalAxis = "Vertical" + playerNumber;
		cursorPointer = Instantiate(cursorPointer);
		ControllerTurningHorizontalAxis = "TurningControllerHorizontal" + playerNumber;
		ControllerTurningVerticalAxis = "TurningControllerVertical" + playerNumber;
		ControllerMovingHorizontalAxis = "MovingControllerHorizontal" + playerNumber;
		ControllerMovingVerticalAxis = "MovingControllerVertical" + playerNumber;

	}
	
	void FixedUpdate () {
		if (playerPrefsManager.controllerInput == 0) {
			if (!cursorPointer.activeInHierarchy) {
				cursorPointer.SetActive (true);
			}
			MoveMouse ();
			TurnMouse ();
		}
		else if (playerPrefsManager.controllerInput == 1) {
			if (cursorPointer.activeInHierarchy) {
				cursorPointer.SetActive (false);
			}
			moveController ();
			TurnController ();
		}
	}

	private void MoveMouse(){
		//The input is retrieved and stored
		HorizontalInput = Input.GetAxis (HorizontalAxis);
		VerticalInput = Input.GetAxis (VerticalAxis);

		//Make the player move (Where z-axis is forwards/backwards and x-axis is sideways). No movement in Y-axis.
		Vector3 MovementInput = new Vector3(HorizontalInput, 0, VerticalInput);

		//Normalize to account for diagonal walking lines
		MovementInput = MovementInput.normalized;

		//Move
		transform.position = transform.position + (MovementInput * speed * Time.deltaTime);
	}

	private void moveController(){
		ControllerMovingHorizontalInput = Input.GetAxis (ControllerMovingHorizontalAxis);
		ControllerMovingVerticalInput = Input.GetAxis (ControllerMovingVerticalAxis);

		transform.position = transform.position + speed * Time.deltaTime * ControllerMovingHorizontalInput * new Vector3(0,0,1);
		transform.position = transform.position + speed * Time.deltaTime * ControllerMovingVerticalInput * new Vector3(1,0,0);
	}

	private void TurnMouse(){

		//Create a ray from the camera through the cursor on the screen (which will hit the floor)
		cameraRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		//Get the point where the ray intersects with the floor, make that lookdirection
		if (Physics.Raycast (cameraRay, out floorHit, cameraRayLength, floorMask)) {
			Vector3 lookDirection = floorHit.point - transform.position;

			//Make y 0 so that the player will not look up or down.
			lookDirection.y = 0f;

			updateCursorPointer (floorHit);

			Quaternion playerRotation = Quaternion.LookRotation (lookDirection);
			transform.rotation = playerRotation;
		}

	}

	private void TurnController(){
		ControllerTurningHorizontalInput = Input.GetAxis (ControllerTurningHorizontalAxis);
		ControllerTurningVerticalInput = Input.GetAxis (ControllerTurningVerticalAxis);

		// We are going to read the input every frame
		Vector3 vNewInput = new Vector3 (ControllerTurningHorizontalInput, 0.0f, ControllerTurningVerticalInput);

		// Only do work if meaningful
		if (vNewInput.sqrMagnitude < 0.1f) {
			return;
		}

		// Apply the transform to the object  
		var angle = Mathf.Atan2 (ControllerTurningHorizontalInput, ControllerTurningVerticalInput) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0, angle, 0);

	}

	//Updates the position of the crosshairs to the cursor position.
	private void updateCursorPointer(RaycastHit hit){
		cursorPointer.transform.position = new Vector3 (hit.point.x, 0.1f, hit.point.z);
	}
}
