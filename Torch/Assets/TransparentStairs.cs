using UnityEngine;
using System.Collections;

public class TransparentStairs : MonoBehaviour {

	public Animator animator;

	void OnTriggerStay(Collider other){
		if(other.CompareTag("Player")){
			animator.SetBool ("Invisible", true);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.CompareTag ("Player")) {
			animator.SetBool ("Invisible", false);
		}
	}
}
