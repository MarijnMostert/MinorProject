using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HomeScreenMovement : MonoBehaviour {

	public LayerMask layerMask;
	public float minimalheight = -2.0f;
	public float maximalheight = 2.5f;

    GameObject target;
    public float rotateSpeed = 5f;
	public float walkingSpeed = .1f;
    Vector3 offset;
    float min_height;
	Vector3 targetlocation;
	float idealdistance;
	Vector3 originalforward;
	public float smoothing = 0.3f;
	Vector3 smoothDampVar;

	public bool invertY = false;

	private string moveHorizontal;
	private string moveVertical;
	private float HorizontalInput;
	private float VerticalInput;

    Animator anim;

    void Awake()
    {
        target = GameObject.Find("HomeScreenPlayer");
        anim = target.GetComponentInChildren<Animator>();
    }

    void Start()
    {
        offset = target.transform.position - transform.position;
		idealdistance = offset.magnitude;
		originalforward = transform.forward;
    }

	// Update is called once per frame
	void Update () {
		Move ();
        /*anim.SetBool("walking", false);
        if (Input.GetKey("d"))
        {
			target.transform.Translate(new Vector3(0, 0, walkingSpeed));
            anim.SetBool("walking",true);
        }
        if (Input.GetKey("a"))
        {
			target.transform.Translate(new Vector3(0, 0, -walkingSpeed));
            anim.SetBool("walking", true);
        }
        if (Input.GetKey("w"))
        {
			target.transform.Translate(new Vector3(-walkingSpeed, 0, 0));
            anim.SetBool("walking", true);
        }
        if (Input.GetKey("s"))
        {
            target. transform.Translate(new Vector3(walkingSpeed, 0, 0));
            anim.SetBool("walking", true);
        }*/
		float horizontal = Input.GetAxis("turnHorizontal1") * rotateSpeed;
		if (Mathf.Abs(horizontal) < .1f){//no controllerinput then use mouse
			horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
		}
        target.transform.Rotate(0, horizontal, 0);

        float desiredAngle = target.transform.eulerAngles.y;
        float vertical = Input.GetAxis("turnVertical1") * rotateSpeed/10f;
		if (Mathf.Abs(vertical) < .1f){//no controllerinput then use mouse
			vertical = Input.GetAxis("Mouse Y") * rotateSpeed/10f;
		}
		//implementation of inverted Y axis
		if (invertY) {
			vertical = -1f * vertical;
		}
        Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
        min_height += vertical;

		if (min_height < minimalheight) {
			min_height = minimalheight;
		} else if (min_height > maximalheight) {
			min_height = maximalheight;
		}

		targetlocation = target.transform.position - (rotation * offset) + new Vector3(0,min_height,0);
    }    

	private void Move(){
		//The input is retrieved and stored
		HorizontalInput = Input.GetAxis ("moveHorizontal1");
		VerticalInput = Input.GetAxis ("moveVertical1");

		//Make the player move (Where z-axis is forwards/backwards and x-axis is sideways). No movement in Y-axis.
		//camera axes relative to player are (-z,y,x)
		Vector3 MovementInput = new Vector3(-VerticalInput, 0, HorizontalInput);

		//Normalize to account for diagonal walking lines
		MovementInput = MovementInput.normalized;

		//Move
		if (anim!=null) {
			
			if ((MovementInput * walkingSpeed * Time.deltaTime).magnitude > .1f)
			{
				anim.SetBool("walking",true);
			} else
			{
				anim.SetBool("walking", false);
			}
		}
		target.transform.Translate(MovementInput * walkingSpeed * Time.deltaTime);
	}

	public void InvertY(bool newBool){
		invertY = newBool;
		if (invertY)
			Debug.Log ("Y-axis inverted");
		else
			Debug.Log ("Y-axis not inverted");
	}

	void LateUpdate() {
		transform.position = Vector3.SmoothDamp (transform.position, targetlocation, ref smoothDampVar, smoothing);
		transform.LookAt (target.transform, Vector3.up * 0.2f);
	}
}
