using UnityEngine;
using System.Collections;

public class HomeScreenMovement : MonoBehaviour {


    GameObject target;
    public float rotateSpeed = 5;
    Vector3 offset;
    float min_height;

    Animator anim;

    void Awake()
    {
        target = GameObject.Find("HomeScreenPlayer");
        anim = target.GetComponentInChildren<Animator>();
    }

    void Start()
    {
        offset = target.transform.position - transform.position;
    }

	// Update is called once per frame
	void Update () {
        anim.SetBool("walking", false);
        if (Input.GetKey("d"))
        {
            target.transform.Translate(new Vector3(0, 0, .05f));
            anim.SetBool("walking",true);
        }
        if (Input.GetKey("a"))
        {
            target.transform.Translate(new Vector3(0, 0, -.05f));
            anim.SetBool("walking", true);
        }
        if (Input.GetKey("w"))
        {
            target.transform.Translate(new Vector3(-.05f, 0, 0));
            anim.SetBool("walking", true);
        }
        if (Input.GetKey("s"))
        {
            target. transform.Translate(new Vector3(.05f, 0, 0));
            anim.SetBool("walking", true);
        }
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        target.transform.Rotate(0, horizontal, 0);

        float desiredAngle = target.transform.eulerAngles.y;
        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed/10f;
        Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
        min_height += vertical;
        transform.position = target.transform.position - (rotation * offset) + new Vector3(0,min_height,0);
    }    

    void LateUpdate(){
		transform.LookAt(target.transform);
	}
}
