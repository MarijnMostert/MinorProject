using UnityEngine;
using System.Collections;

public class HomeScreenMovement : MonoBehaviour {


    GameObject target;
    public float rotateSpeed = 5;
    Vector3 offset;
    float min_height;

    void Awake()
    {
        target = GameObject.Find("HomeScreenPlayer");
    }

    void Start()
    {
        offset = target.transform.position - transform.position;
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKey("d"))
        {
            target.transform.Translate(new Vector3(0, 0, .1f));
        }
        if (Input.GetKey("a"))
        {
            target.transform.Translate(new Vector3(0, 0, -.1f));
        }
        if (Input.GetKey("w"))
        {
            target.transform.Translate(new Vector3(-.1f, 0, 0));
        }
        if (Input.GetKey("s"))
        {
            target. transform.Translate(new Vector3(.1f, 0, 0));
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
