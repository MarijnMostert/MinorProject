using UnityEngine;
using System.Collections;

public class PartyTorch : AudioObject {

	public Light torchLight;
	public ParticleSystem particles;
	public Gradient g;
	public float duration = 2f;
	private Color defaultColor;
	public bool Enabled = true;
	public GameObject Discoball;
	public Vector3 defaultDiscoballPosition;
	public ParticlePlayer confettiParticles;
	public AudioClip cheer;
	public AudioClip fireworks;

	void Awake(){
		defaultColor = torchLight.color;
		defaultDiscoballPosition = Discoball.transform.localPosition;
	}

	// Use this for initialization
	void OnEnable(){
		Enabled = true;
	}

	void OnDisable(){
		Enabled = false;
	}

	void Update(){
		if (Enabled) {
			float t = Mathf.Repeat (Time.time, duration) / duration;
			torchLight.color = g.Evaluate (t);
			particles.startColor = g.Evaluate (1f-t);
		} else {
			torchLight.color = defaultColor;
		}
	}

	void ReparentDiscoball(Transform targetParent, Vector3 localOffset){
		Discoball.transform.parent = targetParent;
		Discoball.transform.localPosition = localOffset;
	}

	void ParentDiscoball(){
		Discoball.transform.parent = transform;
		Discoball.transform.localPosition = defaultDiscoballPosition;
	}

	public void puzzleEnter(Transform parent){
		ReparentDiscoball (transform, new Vector3 (0f, 7f, 0f));
	}

	public void puzzleExit(Transform parent){
		ParentDiscoball ();
		ParticlePlayer PP = Instantiate (confettiParticles, parent.position, Quaternion.identity, parent) as ParticlePlayer;
		PP.Play (1f);
		ObjectPooler.Instance.PlayAudioSource (cheer, mixerGroup, pitchMin, pitchMax, transform);
		ObjectPooler.Instance.PlayAudioSource (fireworks, mixerGroup, pitchMin, pitchMax, transform);
	}
}