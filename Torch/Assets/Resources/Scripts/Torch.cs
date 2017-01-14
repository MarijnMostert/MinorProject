using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Torch : MonoBehaviour, IDamagable {

	public Light torchLight;
	public float intensityMinimum = 0f;
	public float intensityMaximum = 2f;
	public int startingHealth;
	public int health;
	public float rangeMinimum = 5;
	public float rangeMaximum = 40f;
	public float smoothingTime = 1f;
	[HideInInspector] //This variable will be used by other scripts but will not be editable in the Unity GUI.
	public float flickerInterval = 0.5f;

	public int damageOverTimeVarDamage = 2;
	public float damageOverTimeVarTime = 5f;


	private float smoothDampVar1 = 0f;
	private float smoothDampVar2 = 0f;
	private float smoothDampVar3 = 0f;
	private float smoothDampVar4 = 0f;
	private float intensityBase;
	private float rangeBase;
	private float randomFactorIntensity;
	private float randomFactorRange;

	private float randomValue;

	public UI ui;
	private Text healthText;
	private Image healthBar;
	public GameObject HealingParticles;
	public ParticleSystem MainParticles;
	public float minParticleSize;
	public float maxParticleSize;
	[HideInInspector] public Animator TorchFOV;

	[HideInInspector] public GameManager gameManager;
	public TorchPickUp torchPickUp;
	public bool isDamagable = true;

	new void Start () {
		health = startingHealth;

		torchLight = transform.GetComponentInChildren<Light> ();
		HealingParticles.SetActive (false);
		StartCoroutine (DamageOverTime ());

		torchLight.intensity = intensityMaximum;
		intensityBase = intensityMaximum;
		randomFactorIntensity = (intensityMaximum - intensityMinimum) / 8f;
		randomFactorRange = (rangeMaximum - rangeMinimum) / 8f;

		//Coroutine for the flickering of the light.
		StartCoroutine(TorchFlickering());
	}
	
	void Update () {
		lightUpdate ();
	}

	//Update the light intensity and range according to the health
	private void lightUpdate(){
		//Met smoothdamp ga je van de ene waarde geleidelijk over in de andere met een bepaalde smoothingTime.
		rangeBase = (float)health / gameManager.torchStartingHealth * (rangeMaximum - rangeMinimum) + rangeMinimum;
		torchLight.range = Mathf.SmoothDamp(torchLight.range, rangeBase, ref smoothDampVar1, smoothingTime);
		intensityBase = (float)health / gameManager.torchStartingHealth * (intensityMaximum - intensityMinimum) + intensityMaximum;
		torchLight.intensity = Mathf.SmoothDamp(torchLight.intensity, intensityBase, ref smoothDampVar2, smoothingTime);
	}

	//For when the torch takes damage
	public void takeDamage(int damage, bool crit, GameObject source){
		if (isDamagable) {
//		Debug.Log (gameObject + " takes " + damage + " damage.");
			health -= damage;
			updateHealth ();
			TorchFOV.SetTrigger ("TakeDamage");

			if (health <= 0) {
				Debug.Log ("Player dies by taking " + damage + " from " + source.name);
				Die ();
			}
		}
	}

	//For when the player e.g. picks up a healthPickUp.
	public void heal(int healingAmount){
//		Debug.Log (gameObject + " heals " + healingAmount + "points");
		health += healingAmount;
		if (health > gameManager.torchHealthMax) {
			health = gameManager.torchHealthMax;
		}
		updateHealth ();
		StartCoroutine (ParticlesCoroutine ());
	}

	IEnumerator ParticlesCoroutine(){
		HealingParticles.SetActive (true);
		yield return new WaitForSeconds (2.5f);
		HealingParticles.SetActive (false);
	}

	//Random deviation from the base intensity and range.
	IEnumerator TorchFlickering(){
		while(gameObject.activeSelf){
			randomValue = Random.value;
			torchLight.intensity = Mathf.SmoothDamp (torchLight.intensity, intensityBase + ((2f * randomValue) - 1f) * randomFactorIntensity, ref smoothDampVar3, flickerInterval);
			torchLight.range = Mathf.SmoothDamp (torchLight.range, rangeBase + ((2f * randomValue) - 1f) * randomFactorRange, ref smoothDampVar4, flickerInterval);
			yield return new WaitForSeconds (flickerInterval);
		}
	}

	//Update the health of the torch.
	private void updateHealth(){
		if (healthText == null) {
			healthText = ui.currentHealthText;
		}
		healthText.text = health.ToString ();
		gameManager.torchHealth = health;

		if (healthBar == null) {
			healthBar = ui.healthImage;
		}
		healthBar.fillAmount = (float)health / (float)startingHealth * 0.4f + 0.6f;

		MainParticles.startSize = minParticleSize + ((maxParticleSize - minParticleSize) * health / startingHealth);
	}

	public void Die(){
		health = 0;
		Destroy (torchPickUp.canvas);
		Destroy (GameObject.FindGameObjectWithTag("CursorPointer"));
		gameManager.GameOver();
	}

	//Coroutine to receive damage over time
	IEnumerator DamageOverTime(){
		while (gameObject.activeSelf) {
			yield return new WaitForSeconds (damageOverTimeVarTime);
			if (isDamagable) {
				//		Debug.Log (gameObject + " takes " + damage + " damage.");
				health -= damageOverTimeVarDamage;
				updateHealth ();

				if (health <= 0) {
					Debug.Log ("Killed by damage over time on torch");
					Die ();
				}
			}
		}
	}

	//Cheatcode to regain full health
	public void HealToStartingHealth(){
		health = startingHealth;
		updateHealth ();
		Debug.Log ("Health is set to " + startingHealth);
	}

	//Toggle if the torch can receive damage or not.
	public void ToggleDamagable(){
		isDamagable = !isDamagable;
		Debug.Log("Torch isDamagable is set to " + isDamagable);
	}
}
