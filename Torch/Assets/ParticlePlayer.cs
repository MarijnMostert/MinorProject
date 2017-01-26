using UnityEngine;
using System.Collections;

public class ParticlePlayer : MonoBehaviour {

	public ParticleSystem[] ParticleSystems;
	public bool hasEndTime;
	public float endTime;

	public void Play(){
		foreach (ParticleSystem PS in ParticleSystems) {
			PS.Play ();
		}
	}

	public void Play(float endTime){
		StartCoroutine (coroutine (endTime));
	}

	IEnumerator coroutine(float endTime){
		foreach (ParticleSystem PS in ParticleSystems) {
			PS.Play ();
		}

		if (hasEndTime) {
			yield return new WaitForSeconds (endTime);
			foreach (ParticleSystem PS in ParticleSystems) {
				PS.Stop ();
			}
		}
	}
}
