using UnityEngine;
using System.Collections;

public class HomeScreenMovement : MonoBehaviour {

	public Camera cam;

	public float speedForwards = 10f;
	public float speedSideways = 3f;

	private string moveHorizontal = "moveHorizontal1";
	private string moveVertical = "moveVertical1";
	private float inputHorizontal;
	private float inputVertical;

	void Awake(){
		cam = GameObject.Find ("HomeScreenCam").GetComponent<Camera> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		inputHorizontal = -1f * Input.GetAxis (moveHorizontal);
		inputVertical = Input.GetAxis (moveVertical);

		Vector3 forwardsMovement = (transform.position - cam.transform.position).normalized;
		forwardsMovement.y = 0f;

		Vector3 sidewaysMovement = crossProduct ((transform.position - cam.transform.position), transform.up);
		sidewaysMovement.y = 0f;

		transform.position = transform.position + inputVertical * forwardsMovement * speedForwards * Time.deltaTime;
		transform.position = transform.position + inputHorizontal * sidewaysMovement * speedSideways * Time.deltaTime;



	}

	void LateUpdate(){
		cam.transform.LookAt (transform);
	}

	Vector3 crossProduct(Vector3 v1, Vector3 v2){
		return new Vector3 ((v1.y * v2.z - v1.z * v2.y), (v1.z * v2.x - v1.x * v2.z), (v1.x * v2.y - v1.y * v2.x));
	}
}
