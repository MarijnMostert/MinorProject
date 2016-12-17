using UnityEngine;
using System.Collections;

public class SetInactive : MonoBehaviour {

	public float TimeTillInactive = 5f;

	void OnEnable(){
		StartCoroutine (SetInactiveCoroutine ());
	}

	IEnumerator SetInactiveCoroutine(){
		yield return new WaitForSeconds (TimeTillInactive);
		gameObject.SetActive (false);
	}

}
