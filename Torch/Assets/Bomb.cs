﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bomb : AudioObject {

	public float timeTillExplode = 3f;
	public int maxDamage = 120;
	public float blastRange = 3f;
	public ParticlePlayer particles;
	public Canvas ExplosionMark;
	public bool canHitPlayer = false;
	public AudioClip clip;


	// Use this for initialization
	void Start () {
		Invoke ("Explode", timeTillExplode);
	}
	
	void Explode(){
		Collider[] colliders = Physics.OverlapSphere (transform.position, blastRange);
		foreach (Collider collider in colliders) {
			IDamagable damagableObject = collider.GetComponent<IDamagable> ();
			if (damagableObject != null) {
				float distanceFromBomb = Mathf.Abs ((collider.transform.position - transform.position).magnitude);
				int damage = (int)((1 - (distanceFromBomb / blastRange)) * maxDamage);
				if (damage < 0) {
					damage = 0;
				}
				if (canHitPlayer || (!collider.gameObject.CompareTag ("Player") && !collider.gameObject.CompareTag ("Torch"))) {
//					Debug.Log (damage + " damage on " + collider.gameObject);
					damagableObject.takeDamage (damage, false, gameObject);
				}
			}
		}
		particles = Instantiate (particles, transform.position, Quaternion.identity) as ParticlePlayer;
		particles.Play ();

		ObjectPooler.Instance.PlayAudioSource (clip, mixerGroup, pitchMin, pitchMax, transform);

		Vector3 pos = transform.position;
		pos.y = 0.01f;
		Instantiate (ExplosionMark, pos, Quaternion.Euler(new Vector3(-90f, 0f, 0f)));

		Destroy (particles.gameObject, 4f);
		Destroy (gameObject);
	}
}
