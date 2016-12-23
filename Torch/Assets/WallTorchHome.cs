using UnityEngine;
using System.Collections;

public class WallTorchHome : MonoBehaviour {
	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			for (int i = 0; i < 3; i++) {
				transform.GetChild(0).GetChild (i).gameObject.SetActive (true);
		    }
		}
	}

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < 3; i++)
            {
               transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
