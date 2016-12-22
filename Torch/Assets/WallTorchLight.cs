using UnityEngine;
using System.Collections;

public class WallTorchLight : MonoBehaviour {

	private bool enabled = false;
	private Light light;
	public float intensity;
	public float range;
	private float smoothDampVar1;
	private float smoothDampVar2;

	void Awake(){
		light = GetComponent<Light> ();
	}

	void OnEnable(){
		enabled = true;
	}

	void Update(){
		if (enabled) {
			light.intensity = Mathf.SmoothDamp (light.intensity, intensity, ref smoothDampVar1, .5f);
			light.range = Mathf.SmoothDamp (light.range, range, ref smoothDampVar2, .5f);
		}
	}
}
