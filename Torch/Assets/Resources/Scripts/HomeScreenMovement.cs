using UnityEngine;
using System.Collections;

public class HomeScreenMovement : MonoBehaviour {


    GameObject target;
    public float rotateSpeed = 5;
    Vector3 offset;


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
        if (Input.GetKey("s"))
        {
            target.transform.Translate(new Vector3(0, 0, .1f));
        }
        if (Input.GetKey("d"))
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
        Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
        transform.position = target.transform.position - (rotation * offset);
    }

    void LateUpdate(){
		transform.LookAt(target.transform);
	}
}
