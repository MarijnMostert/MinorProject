using UnityEngine;
using System.Collections;

public class toggleTrapDoor : MonoBehaviour {
    bool toggle;
    Vector3 diff = new Vector3(0, 3f, 0);

    // Use this for initialization
    void Start () {
        transform.position -= diff;
        toggle = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("t"))
        {
            toggleDoor();
        }
	}

    void toggleDoor()
    {
        if (toggle) {
            transform.position -= diff;
        } else {
            transform.position += diff;
        }
        toggle = !toggle;
    }
}
