using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class HealthPickUp : AudioObject, IPickUp {

	public int healAmount = 10;
	public AudioClip clip;

	//Heals the torch when picked up
	public void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			GameManager.Instance.torch.heal (healAmount);
			ObjectPooler.Instance.PlayAudioSource (clip, mixerGroup, pitchMin, pitchMax, transform);
			Destroy (transform.parent.gameObject);
		}
	}
}
