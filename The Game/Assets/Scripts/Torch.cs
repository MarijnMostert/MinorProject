using UnityEngine;
using System.Collections;

public class Torch : MonoBehaviour {

	public int startingHealth;
	public Light torchLight;
	public float startingIntensity = 4f;
	public int health;
	public float range = 30f;
	public float smoothingTime = 1f;
	// public float flickerInterval = 0.5f;
	private float smoothDampVar = 0f;
	private float torchLightBase;
	// private float randomFactor;

	void Start () {
		torchLight = transform.GetComponentInChildren<Light> ();
		torchLight.intensity = startingIntensity;
		torchLightBase = torchLight.intensity;
	//	randomFactor = startingIntensity / 20f;

		health = startingHealth;

	//	InvokeRepeating ("torchFlickering", 0f, flickerInterval);
	}
	
	void Update () {

		lightUpdate ();

		if (isDead ()) {
			CancelInvoke ();
			Destroy (transform.parent.gameObject);
		}
	}

	//Update the light intensity and range according to the health
	private void lightUpdate(){
		//Met smoothdamp ga je van de ene waarde geleidelijk over in de andere met een bepaalde smoothingTime.
		torchLight.range = Mathf.SmoothDamp(torchLight.range, (float)health / startingHealth * range + 20f, ref smoothDampVar, smoothingTime);
		torchLightBase = (float)health / startingHealth * startingIntensity;
		torchLight.intensity = Mathf.SmoothDamp(torchLight.intensity, torchLightBase, ref smoothDampVar, smoothingTime);
	}

	//Check if the player is dead (health 0 or below)
	private bool isDead(){
		if (health <= 0) {
			return true;
		}
		return false;
	}

	public void takeDamage(int damage){
		health -= damage;
	}

	/*
	private void torchFlickering(){

		torchLight.intensity = Mathf.SmoothDamp (torchLight.intensity, torchLightBase + ((2f * Random.value) - 1f) * randomFactor, ref smoothDampVar, flickerInterval);

	}
	*/
}
