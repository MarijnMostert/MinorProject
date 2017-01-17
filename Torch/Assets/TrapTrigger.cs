using UnityEngine;
using System.Collections;

public class TrapTrigger : MonoBehaviour {
    Trap[] traps; 

	// Use this for initialization
	void Start () {
        traps = GetComponentsInChildren<Trap>();
    }
	
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("triggered trap");
            foreach(Trap tmp in traps)
            {
                tmp.trigger();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach(Trap tmp in traps)
            {
                tmp.endtrigger();
            }
        }
    }
}
