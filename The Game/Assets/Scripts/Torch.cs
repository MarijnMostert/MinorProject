using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Torch : MonoBehaviour {

	public int startingHealth;
	public Light torchLight;
	public float startingIntensity = 4f;
	public int health;
	public float range = 5f;
	public float smoothingTime = 1f;
	public Text healthText;
	public Text deathText;
	[HideInInspector]
	public bool dead;
	public float flickerInterval = 0.5f;


	private float smoothDampVar1 = 0f;
	private float smoothDampVar2 = 0f;
	private float smoothDampVar3 = 0f;
	private float smoothDampVar4 = 0f;
	private float intensityBase;
	private float rangeBase;
	private float randomFactorIntensity;
	private float randomFactorRange;

	void Start () {
		torchLight = transform.GetComponentInChildren<Light> ();
		torchLight.intensity = startingIntensity;
		intensityBase = startingIntensity;
		randomFactorIntensity = startingIntensity / 8f;
		randomFactorRange = range / 8f;

		health = startingHealth;
		healthText.text = "Health: " + health;

		InvokeRepeating ("torchFlickering", 0f, flickerInterval);
	}
	
	void Update () {
		lightUpdate ();
	}

	//Update the light intensity and range according to the health
	private void lightUpdate(){
		//Met smoothdamp ga je van de ene waarde geleidelijk over in de andere met een bepaalde smoothingTime.
		rangeBase = (float)health / startingHealth * range + 20f;
		torchLight.range = Mathf.SmoothDamp(torchLight.range, rangeBase, ref smoothDampVar1, smoothingTime);
		intensityBase = (float)health / startingHealth * startingIntensity;
		torchLight.intensity = Mathf.SmoothDamp(torchLight.intensity, intensityBase, ref smoothDampVar2, smoothingTime);
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
		healthText.text = "Health: " + health;

		if (isDead ()) {
			dead = true;
			CancelInvoke ();
			transform.parent.gameObject.SetActive(false);
			deathText.gameObject.SetActive(true);
		}
	}
		
	private void torchFlickering(){
		float randomValue = Random.value;
		torchLight.intensity = Mathf.SmoothDamp (torchLight.intensity, intensityBase + ((2f * randomValue) - 1f) * randomFactorIntensity, ref smoothDampVar3, flickerInterval);
		torchLight.range = Mathf.SmoothDamp (torchLight.range, rangeBase + ((2f * randomValue) - 1f) * randomFactorRange, ref smoothDampVar4, flickerInterval);

	}

}
