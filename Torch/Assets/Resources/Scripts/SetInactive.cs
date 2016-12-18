using UnityEngine;
using System.Collections;

public class SetInactive : MonoBehaviour {

	public float TimeTillInactive = 5f;

	void OnEnable(){
		Invoke ("Destroy", TimeTillInactive);
	}

	void OnDisable(){
		CancelInvoke ();
	}

	void Destroy(){
		gameObject.SetActive (false);
	}

}
