using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public int playerNumber;
	public float speed;
	public LayerMask floorMask;
	public GameObject cursorPointer;
<<<<<<< HEAD:The Game/Assets/Resources/Scripts/PlayerMovement.cs
=======
	public Camera mainCamera;
	public int playerNumber;
	public bool controllerInput = false;
>>>>>>> master:Torch/Assets/Resources/Scripts/PlayerMovement.cs

	private string HorizontalAxis;
	private string VerticalAxis;
	private float HorizontalInput;
	private float VerticalInput;
<<<<<<< HEAD:The Game/Assets/Resources/Scripts/PlayerMovement.cs
=======

	private string turnHorizontal;
	private string turnVertical;
	private float turnHorizontalInput;
	private float turnVerticalInput;

>>>>>>> master:Torch/Assets/Resources/Scripts/PlayerMovement.cs
	private float cameraRayLength = 200f;
	private RaycastHit floorHit;
	private Ray cameraRay;

<<<<<<< HEAD:The Game/Assets/Resources/Scripts/PlayerMovement.cs
	public PlayerPrefsManager ppM;
=======
	private GameObject controllerButtons;
	private GameObject keyboardButtons;

	[SerializeField] private float velocity;
	private Vector3 prevPos = new Vector3 (0, 0, 0);

	void Awake(){
		controllerButtons = GameObject.Find ("Controller Buttons");
		controllerButtons.SetActive (false);
		keyboardButtons = GameObject.Find ("Key Buttons");
	}

	/*
	private string HorizontalAxis;
	private string VerticalAxis;
>>>>>>> master:Torch/Assets/Resources/Scripts/PlayerMovement.cs

	private string ControllerTurningHorizontalAxis;
	private string ControllerTurningVerticalAxis;
	private float ControllerTurningHorizontalInput;
	private float ControllerTurningVerticalInput;
	private string ControllerMovingHorizontalAxis;
	private string ControllerMovingVerticalAxis;
	private float ControllerMovingHorizontalInput;
	private float ControllerMovingVerticalInput;

	void Awake(){
		ppM = GameObject.Find ("SceneManager").GetComponent<PlayerPrefsManager> ();
		if (ppM == null)
			Debug.Log ("Add Scene Manager prefab to the scene");
		cursorPointer = Instantiate (cursorPointer);
	}

	void Start () {

		//The input may differ for another player (e.g. arrow keys vs. wasd keys)
		/*
		HorizontalAxis = "Horizontal" + playerNumber;
		VerticalAxis = "Vertical" + playerNumber;
		cursorPointer = Instantiate(cursorPointer);
		ControllerTurningHorizontalAxis = "TurningControllerHorizontal" + playerNumber;
		ControllerTurningVerticalAxis = "TurningControllerVertical" + playerNumber;
		ControllerMovingHorizontalAxis = "MovingControllerHorizontal" + playerNumber;
		ControllerMovingVerticalAxis = "MovingControllerVertical" + playerNumber;
		*/

<<<<<<< HEAD:The Game/Assets/Resources/Scripts/PlayerMovement.cs
	}
	
	void FixedUpdate () {
		if (ppM.controllerInput == 0) {
			if (!cursorPointer.activeInHierarchy) {
				cursorPointer.SetActive (true);
			}
			MoveMouse ();
			TurnMouse ();
		}
		else if (ppM.controllerInput == 1) {
			if (cursorPointer.activeInHierarchy) {
				cursorPointer.SetActive (false);
			}
			moveController ();
			TurnController ();
		}
=======
    public void setMoves(int playerNumber)
    {
        this.playerNumber = playerNumber;
        moveHorizontal = "moveHorizontal" + playerNumber;
        moveVertical = "moveVertical" + playerNumber;

		turnHorizontal = "turnHorizontal" + playerNumber;
		turnVertical = "turnVertical" + playerNumber;
    }

	void Update(){
		if (Input.GetButtonDown("ToggleInput" + playerNumber)) {
			if (controllerInput) {
				controllerInput = false;
				controllerButtons.SetActive (false);
				keyboardButtons.SetActive (true);
			} else {
				controllerInput = true;
				controllerButtons.SetActive (true);
				keyboardButtons.SetActive (false);
			}
		}
	}
	
	void FixedUpdate () {
		Move ();
		if (controllerInput) {
			TurnController ();
			updateCursorPointer (transform.position + transform.forward * 5f);
		} else {
			Turn ();
		}
		UpdateVelocity ();
>>>>>>> master:Torch/Assets/Resources/Scripts/PlayerMovement.cs
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

	private void moveController(){
		ControllerMovingHorizontalInput = Input.GetAxis (ppM.moveHorizontalAxis);
		ControllerMovingVerticalInput = Input.GetAxis (ppM.moveVerticalAxis);

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

			updateCursorPointer (floorHit.point);

			Quaternion playerRotation = Quaternion.LookRotation (lookDirection);
			transform.rotation = playerRotation;
		}
	}

	private void TurnController(){
		turnHorizontalInput = Input.GetAxis (turnHorizontal);
		turnVerticalInput = Input.GetAxis (turnVertical);

		// We are going to read the input every frame
		Vector3 input = new Vector3 (turnHorizontalInput, 0.0f, turnVerticalInput);

		if (input.sqrMagnitude < 0.1f) {
			return;
		}

		// Apply the transform to the object  
		var angle = Mathf.Atan2 (turnHorizontalInput, turnVerticalInput) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0, angle, 0);
	}

<<<<<<< HEAD:The Game/Assets/Resources/Scripts/PlayerMovement.cs
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

=======
	//Updates the position of the crosshairs to the cursor position.
	private void updateCursorPointer(Vector3 position){
		cursorPointer.transform.position = new Vector3 (position.x, 0.1f, position.z);
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
>>>>>>> master:Torch/Assets/Resources/Scripts/PlayerMovement.cs
	}

	//Updates the position of the crosshairs to the cursor position.
	private void updateCursorPointer(RaycastHit hit){
		cursorPointer.transform.position = new Vector3 (hit.point.x, 0.1f, hit.point.z);
	}
}
