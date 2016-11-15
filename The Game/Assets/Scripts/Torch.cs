using UnityEngine;
using System.Collections;

public class Torch : MonoBehaviour {

	public int startingHealth;
	public Light torchLight;
	public float intensity = 4f;
	public int health;
	public float range = 30f;

	void Start () {
		torchLight = transform.GetComponentInChildren<Light> ();
		health = startingHealth;
	}
	
	void Update () {

		lightUpdate ();

		if (isDead ()) {
			Destroy (transform.parent.gameObject);
		}
			
	}

	private void lightUpdate(){
		torchLight.range = (float)health / startingHealth * range + 20f;
		torchLight.intensity = (float)health / startingHealth * intensity;
	}


	private bool isDead(){
		if (health <= 0) {
			return true;
		}
		return false;
	}
}
