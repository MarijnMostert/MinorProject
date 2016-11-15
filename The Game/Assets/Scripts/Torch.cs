using UnityEngine;
using System.Collections;

public class Torch : MonoBehaviour {

	public int startingHealth;
	public Light torchLight;
	public float intensity = 4f;
	public int health;
	public float range = 30f;
	public float smoothingTime = 1f;
	private float smoothDampVar = 0f;

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

	//Update the light intensity and range according to the health
	private void lightUpdate(){
		//Met smoothdamp ga je van de ene waarde geleidelijk over in de andere met een bepaalde smoothingTime.
		torchLight.range = Mathf.SmoothDamp(torchLight.range, (float)health / startingHealth * range + 20f, ref smoothDampVar, smoothingTime);
		torchLight.intensity = Mathf.SmoothDamp(torchLight.intensity, (float)health / startingHealth * intensity, ref smoothDampVar, smoothingTime);
	}

	//Check if the player is dead (health 0 or below)
	private bool isDead(){
		if (health <= 0) {
			return true;
		}
		return false;
	}
}
