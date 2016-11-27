using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speed;
	public LayerMask floorMask;
	public GameObject cursorPointer;
	[HideInInspector] public Camera mainCamera;
	[HideInInspector] public int playerNumber;

	private string moveHorizontal;
	private string moveVertical;
	private float HorizontalInput;
	private float VerticalInput;

	private float cameraRayLength = 200f;
	private RaycastHit floorHit;
	private Ray cameraRay;

	[SerializeField] private float velocity;
	private Vector3 prevPos = new Vector3 (0, 0, 0);

	/*
	private string HorizontalAxis;
	private string VerticalAxis;

	private string ControllerTurningHorizontalAxis;
	private string ControllerTurningVerticalAxis;
	private float ControllerTurningHorizontalInput;
	private float ControllerTurningVerticalInput;
	private string ControllerMovingHorizontalAxis;
	private string ControllerMovingVerticalAxis;
	private float ControllerMovingHorizontalInput;
	private float ControllerMovingVerticalInput;
	*/


	void Start () {

		moveHorizontal = "moveHorizontal" + playerNumber;
		moveVertical = "moveVertical" + playerNumber;

		cursorPointer = Instantiate(cursorPointer);

		/*
		HorizontalAxis = "Horizontal" + playerNumber;
		VerticalAxis = "Vertical" + playerNumber;
		ControllerTurningHorizontalAxis = "TurningControllerHorizontal" + playerNumber;
		ControllerTurningVerticalAxis = "TurningControllerVertical" + playerNumber;
		ControllerMovingHorizontalAxis = "MovingControllerHorizontal" + playerNumber;
		ControllerMovingVerticalAxis = "MovingControllerVertical" + playerNumber;
		*/

	}
	
	void FixedUpdate () {
		Move ();
		Turn ();
		UpdateVelocity ();
	}
		
	private void Move(){
		//The input is retrieved and stored
		HorizontalInput = Input.GetAxis (moveHorizontal);
		VerticalInput = Input.GetAxis (moveVertical);

		//Make the player move (Where z-axis is forwards/backwards and x-axis is sideways). No movement in Y-axis.
		Vector3 MovementInput = new Vector3(HorizontalInput, 0, VerticalInput);

		//Normalize to account for diagonal walking lines
		MovementInput = MovementInput.normalized;

		//Move
		transform.position = transform.position + (MovementInput * speed * Time.deltaTime);
	}

	private void Turn(){

		//Create a ray from the camera through the cursor on the screen (which will hit the floor)
		cameraRay = mainCamera.ScreenPointToRay (Input.mousePosition);

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

	//Updates the position of the crosshairs to the cursor position.
	private void updateCursorPointer(RaycastHit hit){
		cursorPointer.transform.position = new Vector3 (hit.point.x, 0.1f, hit.point.z);
	}

	void UpdateVelocity(){
		Vector3 currentPos = transform.position;
		velocity = (currentPos - prevPos).magnitude / Time.deltaTime;

		prevPos = currentPos;
	}
		
}








//OUDE CONTROLS.
//WORDEN NIET GEBRUIKT OP DIT MOMENT.

/*
	private void TurnController(){
		ControllerTurningHorizontalInput = Input.GetAxis (ppM.turnHorizontalAxis);
		ControllerTurningVerticalInput = Input.GetAxis (ppM.turnVerticalAxis);

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

	
	private void moveController(){
		ControllerMovingHorizontalInput = Input.GetAxis (ppM.moveHorizontalAxis);
		ControllerMovingVerticalInput = Input.GetAxis (ppM.moveVerticalAxis);

		transform.position = transform.position + speed * Time.deltaTime * ControllerMovingHorizontalInput * new Vector3(0,0,1);
		transform.position = transform.position + speed * Time.deltaTime * ControllerMovingVerticalInput * new Vector3(1,0,0);
	}

	private void MoveMouse(){
		//The input is retrieved and stored
		HorizontalInput = Input.GetAxis (ppM.moveHorizontalAxis);
		VerticalInput = Input.GetAxis (ppM.moveVerticalAxis);

		//Make the player move (Where z-axis is forwards/backwards and x-axis is sideways). No movement in Y-axis.
		Vector3 MovementInput = new Vector3(HorizontalInput, 0, VerticalInput);

		//Normalize to account for diagonal walking lines
		MovementInput = MovementInput.normalized;

		//Move
		transform.position = transform.position + (MovementInput * speed * Time.deltaTime);
	}




*/