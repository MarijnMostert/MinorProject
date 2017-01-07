using UnityEngine;
using System.Collections;

public class ParticlePlayer : MonoBehaviour {

	public ParticleSystem[] ParticleSystems;

	public void Play(){
		foreach (ParticleSystem PS in ParticleSystems) {
			PS.Play ();
		}
	}
}
