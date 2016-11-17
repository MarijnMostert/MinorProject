using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public int playerNumber;
	public float speed;
	public LayerMask floorMask;
	public GameObject cursorPointer;

	private string HorizontalAxis;
	private string VerticalAxis;
	private float HorizontalInput;
	private float VerticalInput;
	private float cameraRayLength = 200f;
	private RaycastHit floorHit;
	private Ray cameraRay;


	void Awake(){
	}

	void Start () {
		//The input may differ for another player (e.g. arrow keys vs. wasd keys)
		HorizontalAxis = "Horizontal" + playerNumber;
		VerticalAxis = "Vertical" + playerNumber;
		cursorPointer = Instantiate(cursorPointer);
	}
	
	void FixedUpdate () {
		Move ();
		Turn ();
	}

	private void Move(){
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

	private void Turn(){

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

	//Updates the position of the crosshairs to the cursor position.
	private void updateCursorPointer(RaycastHit hit){
		cursorPointer.transform.position = new Vector3 (hit.point.x, 0.1f, hit.point.z);
	}
}
