using UnityEngine;
using System.Collections;

public class ParticleActivator : MonoBehaviour {

	[SerializeField] private ParticleSystem particles;

	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			particles.Play ();
		}
	}

	void OnTriggerExit(Collider other){
		if (other.CompareTag ("Player")) {
			particles.Stop ();
		}
	}
}
