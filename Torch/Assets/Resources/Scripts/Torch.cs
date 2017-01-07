using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Torch : InteractableItem, IDamagable {

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
	public float intensityBase;
	public float rangeBase;
	public float randomFactorIntensity;
	public float randomFactorRange;

	private float randomValue;

	public GameObject UI;
	private Text healthText;
	private Image healthBar;
	public GameObject HealingParticles;
	public ParticleSystem MainParticles;
	public Animator TorchFOV;
	public float[] TorchFOVSize = {3000,3000};

	public GameManager gameManager;
	public bool equipped = false;
	public bool isDamagable = true;

	new void Start () {
		base.Start ();

		health = startingHealth;

		torchLight = transform.GetComponentInChildren<Light> ();
		HealingParticles = transform.Find ("Particles").gameObject;
		HealingParticles.SetActive (false);
		StartCoroutine (DamageOverTime ());

		torchLight.intensity = intensityMaximum;
		intensityBase = intensityMaximum;
		randomFactorIntensity = (intensityMaximum - intensityMinimum) / 8f;
		randomFactorRange = (rangeMaximum - rangeMinimum) / 8f;

		canvas.SetActive (true);

		//If the number of players is 1, the torch is automatically picked up by player 1
		if (gameManager.numberOfPlayers == 1) {
			pickUpTorch (gameManager.playerManagers [0].playerInstance);
		}

		//Coroutine for the flickering of the light.
		StartCoroutine(TorchFlickering());
	}
	
	void Update () {
		lightUpdate ();
		if (Input.GetButtonDown("DropTorch1") && equipped && gameManager.playerManagers[0].playerInstance.GetComponentInChildren<Torch>() != null) {
			releaseTorch ();
		}
		if (Input.GetButtonDown ("DropTorch2") && equipped && gameManager.playerManagers [1].playerInstance.GetComponentInChildren<Torch>() != null) {
			releaseTorch ();
		}
		if (transform.position.y < -12) {
			Die ();
		}
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
	public void takeDamage(int damage, bool crit){
		if (isDamagable) {
//		Debug.Log (gameObject + " takes " + damage + " damage.");
			health -= damage;
			updateHealth ();
			TorchFOV.SetTrigger ("TakeDamage");

			if (health <= 0) {
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
			healthText = UI.transform.Find ("Health Text").GetComponent<Text> ();
		}
		healthText.text = "Health: " + health;
		gameManager.torchHealth = health;

		if (healthBar == null) {
			healthBar = UI.transform.Find ("HealthBar").Find("Torch Healthbar Fill").GetComponent<Image> ();
		}
		healthBar.fillAmount = (float)health / (float)startingHealth * 0.4f + 0.6f;

		MainParticles.startSize = 0.5f + (0.5f * health / startingHealth);
	}

	public void Die(){
		Debug.Log ("Player dies");
		health = 0;
		Destroy (canvas);
		Destroy (GameObject.FindGameObjectWithTag("CursorPointer"));
		gameManager.GameOver();
	}

	public override void action(GameObject triggerObject){
		pickUpTorch (triggerObject);
	}

	void pickUpTorch(GameObject triggerObject){
		Debug.Log ("Torch is picked up");
		gameManager.WriteTorchPickup ();
		transform.SetParent (triggerObject.transform.FindChild("Torch Holder"));
		transform.position = transform.parent.position;
		transform.rotation = transform.parent.rotation;
		gameManager.enemyTarget = triggerObject;
		gameManager.camTarget = triggerObject;
		canvas.SetActive (false);
		equipped = true;
	}

	public void releaseTorch(){
		Debug.Log ("Torch is dropped");
		transform.parent = null;
		gameManager.enemyTarget = gameObject;
		gameManager.camTarget = gameObject;
		equipped = false;
		canvas.transform.position = new Vector3 (transform.position.x, floatingHeight, transform.position.z);
		canvas.SetActive (true);
	}

	protected override void OnTriggerStay(Collider other){
		if (other.gameObject.CompareTag ("Player")&&canvas!=null) {
			if (Input.GetButtonDown (interactionButton)) {
				action (other.gameObject);
				canvas.gameObject.SetActive (false);
			}
		}
	}

	protected override void OnTriggerExit(Collider other){
		
	}

	//Coroutine to receive damage over time
	IEnumerator DamageOverTime(){
		while (gameObject.activeSelf) {
			if (isDamagable) {
				//		Debug.Log (gameObject + " takes " + damage + " damage.");
				health -= damageOverTimeVarDamage;
				updateHealth ();

				if (health <= 0) {
					Die ();
				}
			}
			yield return new WaitForSeconds (damageOverTimeVarTime);
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
