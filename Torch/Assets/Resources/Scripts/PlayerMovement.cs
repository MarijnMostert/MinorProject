using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

	public float speed;
	public float shiftfactor = 0.3f;
	public LayerMask floorMask;
	[HideInInspector] public GameObject cursorPointerPrefab;
	[SerializeField] private GameObject cursorPointer;
	[HideInInspector] public Color playerColor;
	public Image playerIndicator;
	public Camera mainCamera;
	public int playerNumber;
	public bool controllerInput = false;

	private string moveHorizontal;
	private string moveVertical;
	private float HorizontalInput;
	private float VerticalInput;

	private string turnHorizontal;
	private string turnVertical;
	private float turnHorizontalInput;
	private float turnVerticalInput;

	private float cameraRayLength = 200f;
	private RaycastHit floorHit;
	private Ray cameraRay;

	private UIInventory uiInventory;
	private Image[] playerControllerButtons;
	private Image[] playerKeyboardButtons;
	private Image[] generalControllerButtons;
	private Image[] generalKeyboardButtons;

	public bool godMode = false;

	[SerializeField] private float velocity;
	private Vector3 prevPos;
	public float distanceTravelled;

    Animator anim1;
	private GameManager gameManager;

	public PlayerWeaponController playerWeaponController;

	void Awake(){

		/*
		controllerButtons = GameObject.FindGameObjectsWithTag("UI Help Controller");
		foreach(GameObject obj in controllerButtons){
			obj.SetActive (false);
		}
		keyboardButtons = GameObject.FindGameObjectsWithTag("UI Help Key");
		*/
        anim1 = GetComponentInChildren<Animator>();
		gameManager = GameManager.Instance;
	}

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

	void OnEnable(){
		cursorPointer.SetActive (true);
		prevPos = transform.position;
	}

	void OnDisable(){
		if (cursorPointer != null) {
			cursorPointer.SetActive (false);
		}
	}

	void Start () {
		playerWeaponController = gameObject.GetComponent<PlayerWeaponController> ();
		//moveHorizontal = "moveHorizontal" + playerNumber;
		//moveVertical = "moveVertical" + playerNumber;

		cursorPointer = Instantiate(cursorPointerPrefab);
		cursorPointer.GetComponent<SpriteRenderer> ().color = playerColor;
		cursorPointer.SetActive (true);
		playerIndicator.color = playerColor;

		//setup controller and key buttons in UI.
		uiInventory = UIInventory.Instance;

		if (playerNumber == 1) {
			playerControllerButtons = uiInventory.ControllerButtonsP1;
			playerKeyboardButtons = uiInventory.KeyboardButtonsP1;
			generalControllerButtons = uiInventory.ControllerButtonsGeneral;
			generalKeyboardButtons = uiInventory.KeyboardButtonsGeneral;
			foreach (Image img in playerControllerButtons) {
				img.gameObject.SetActive (false);
			}
		}

		if (playerNumber == 2)
			ToggleInput ();

		/*
		HorizontalAxis = "Horizontal" + playerNumber;
		VerticalAxis = "Vertical" + playerNumber;
		ControllerTurningHorizontalAxis = "TurningControllerHorizontal" + playerNumber;
		ControllerTurningVerticalAxis = "TurningControllerVertical" + playerNumber;
		ControllerMovingHorizontalAxis = "MovingControllerHorizontal" + playerNumber;
		ControllerMovingVerticalAxis = "MovingControllerVertical" + playerNumber;
		*/
	}

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
			ToggleInput ();
		}
		if (Input.GetKeyDown (KeyCode.G) && gameManager.getCheat()) {
			GodMode ();
		}
		if (Input.GetKeyDown(KeyCode.LeftShift)) {
			speed *= shiftfactor;
		}
		if (Input.GetKeyUp(KeyCode.LeftShift)) {
			speed /= shiftfactor;
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
        if (anim1!=null) {
//            Debug.Log((MovementInput * speed * Time.deltaTime).magnitude);
            if ((MovementInput * speed * Time.deltaTime).magnitude > .1f)
            {
                anim1.SetBool("walking",true);
            } else
            {
                anim1.SetBool("walking", false);
            }
        }
		transform.position += (MovementInput * speed * Time.deltaTime);
	}

	private void Turn(){

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
		playerWeaponController.Attack ();
		// Apply the transform to the object  
		var angle = Mathf.Atan2 (turnHorizontalInput, turnVerticalInput) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0, angle, 0);
	}

	//Updates the position of the crosshairs to the cursor position.
	private void updateCursorPointer(Vector3 position){
		cursorPointer.transform.position = position;
	}

	void ToggleInput () {
		controllerInput = !controllerInput;
		if (playerNumber == 1) {
			foreach (Image img in playerControllerButtons) {
				img.gameObject.SetActive (controllerInput);
			}
			//Check if second player is active. If it is, general controller buttons should not be toggled.
			if (!GameManager.Instance.playerManagers [1].active) {
				foreach (Image img in generalControllerButtons) {
					img.gameObject.SetActive (controllerInput);
				}
			}
			foreach (Image img in playerKeyboardButtons) {
				img.gameObject.SetActive (!controllerInput);
			}
			foreach (Image img in generalKeyboardButtons) {
				img.gameObject.SetActive (!controllerInput);
			}
		}
	}

	void UpdateVelocity(){
		float distance = (transform.position - prevPos).magnitude;
		distanceTravelled += distance;
		velocity = distance / Time.deltaTime;
		prevPos = transform.position;
	}

	void GodMode(){
		if (!godMode) {
			Debug.Log ("Godmode turned on");
			GetComponent<Collider> ().enabled = false;
			GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
			speed *= 3;
			godMode = true;
		} else {
			Debug.Log ("Godmode turned off");
			GetComponent<Collider> ().enabled = true;
			GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			speed /= 3;
			godMode = false;
		}
	}

	public void SetColor(Color color){
		cursorPointer.GetComponent<SpriteRenderer> ().color = color;
		playerIndicator.color = color;
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