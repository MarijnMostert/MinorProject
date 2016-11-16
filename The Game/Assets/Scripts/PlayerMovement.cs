using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public int playerNumber;
	public float speed;
	public LayerMask floorMask;

	private Camera mainCamera;
	private string HorizontalAxis;
	private string VerticalAxis;
	private float HorizontalInput;
	private float VerticalInput;
	private float cameraRayLength = 200f;
	private RaycastHit floorHit;
	Ray cameraRay;


	void Awake(){
		mainCamera = Camera.current;
	}

	void Start () {
		//The input may differ for another player (e.g. arrow keys vs. wasd keys)
		HorizontalAxis = "Horizontal" + playerNumber;
		VerticalAxis = "Vertical" + playerNumber;
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

		//If the ray intersects with the floor, update the lookdirection
		if (Physics.Raycast (cameraRay, out floorHit, cameraRayLength, floorMask)) {
			Vector3 lookDirection = floorHit.point - transform.position;
			lookDirection.y = 0f;

			Quaternion playerRotation = Quaternion.LookRotation (lookDirection);
			transform.rotation = playerRotation;
		}

	}
}
