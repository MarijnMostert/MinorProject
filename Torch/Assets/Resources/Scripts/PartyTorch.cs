using UnityEngine;
using System.Collections;

public class PartyTorch : MonoBehaviour {

	public Light torchLight;
	public ParticleSystem particles;
	public Gradient g;
	public float duration = 2f;
	private Color defaultColor;
	public bool enabled = true;

	void Awake(){
		defaultColor = torchLight.color;
	}

	// Use this for initialization
	void OnEnable(){
		enabled = true;
	}

	void OnDisable(){
		enabled = false;
	}

	void Update(){
		if (enabled) {
			float t = Mathf.Repeat (Time.time, duration) / duration;
			torchLight.color = g.Evaluate (t);
			particles.startColor = g.Evaluate (1f-t);
		} else {
			torchLight.color = defaultColor;
		}
	}
}