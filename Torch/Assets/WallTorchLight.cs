using UnityEngine;
using System.Collections;

public class WallTorchLight : MonoBehaviour {

	private bool Enabled = false;
	private Light Light;
	public float intensity;
	public float range;
	private float smoothDampVar1;
	private float smoothDampVar2;

	void Awake(){
		Light = GetComponent<Light> ();
	}

	void OnEnable(){
		Enabled = true;
	}

	void Update(){
		if (Enabled) {
			Light.intensity = Mathf.SmoothDamp (Light.intensity, intensity, ref smoothDampVar1, .5f);
			Light.range = Mathf.SmoothDamp (Light.range, range, ref smoothDampVar2, .5f);
		}
	}
}
