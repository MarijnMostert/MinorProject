using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Torch : InteractableItem, IDamagable {

	public Light torchLight;
	public float startingIntensity = 4f;
	public int health;
	public float range = 5f;
	public float smoothingTime = 1f;
	[HideInInspector] //This variable will be used by other scripts but will not be editable in the Unity GUI.
	public float flickerInterval = 0.5f;


	private float smoothDampVar1 = 0f;
	private float smoothDampVar2 = 0f;
	private float smoothDampVar3 = 0f;
	private float smoothDampVar4 = 0f;
	public float intensityBase;
	public float rangeBase;
	public float randomFactorIntensity;
	public float randomFactorRange;

	public Canvas UI;
	private Text healthText;

	public GameManager gameManager;
	public bool equipped = false;

	void Awake(){
		
	}

	new void Start () {
		base.Start ();

		torchLight = transform.GetComponentInChildren<Light> ();
		healthText = UI.transform.FindChild("Health Text").GetComponent<Text>();

		torchLight.intensity = startingIntensity;
		intensityBase = startingIntensity;
		randomFactorIntensity = startingIntensity / 8f;
		randomFactorRange = range / 8f;

		healthText.text = "Health: " + health;

		//Every 'flickerInterval' seconds the 'torchFlickering()' function is called.
		InvokeRepeating ("torchFlickering", 0f, flickerInterval);
	}
	
	void Update () {
		lightUpdate ();
	}

	//Update the light intensity and range according to the health
	private void lightUpdate(){
		//Met smoothdamp ga je van de ene waarde geleidelijk over in de andere met een bepaalde smoothingTime.
		rangeBase = (float)health / gameManager.torchStartingHealth * range + 20f;
		torchLight.range = Mathf.SmoothDamp(torchLight.range, rangeBase, ref smoothDampVar1, smoothingTime);
		intensityBase = (float)health / gameManager.torchStartingHealth * startingIntensity;
		torchLight.intensity = Mathf.SmoothDamp(torchLight.intensity, intensityBase, ref smoothDampVar2, smoothingTime);
	}

	//For when the torch takes damage
	public void takeDamage(int damage){
		Debug.Log (gameObject + " takes " + damage + " damage.");
		health -= damage;
		updateHealth ();

		if (health <= 0) {
			Die ();
		}
	}

	//For when the player e.g. picks up a healthPickUp.
	public void heal(int healingAmount){
		Debug.Log (gameObject + " heals " + healingAmount + "points");
		health += healingAmount;
		updateHealth ();
	}
		
	//Random deviation from the base intensity and range.
	private void torchFlickering(){
		float randomValue = Random.value;
		torchLight.intensity = Mathf.SmoothDamp (torchLight.intensity, intensityBase + ((2f * randomValue) - 1f) * randomFactorIntensity, ref smoothDampVar3, flickerInterval);
		torchLight.range = Mathf.SmoothDamp (torchLight.range, rangeBase + ((2f * randomValue) - 1f) * randomFactorRange, ref smoothDampVar4, flickerInterval);
	}

	//Update the health of the torch.
	private void updateHealth(){
		healthText.text = "Health: " + health;
		gameManager.torchHealth = health;
	}

	public void Die(){
		Debug.Log ("Player dies");
		health = 0;
		CancelInvoke ();
		gameManager.GameOver ();
	}

	public override void action(GameObject triggerObject){
		if (equipped) {
			dropTorch ();
		} else {
			pickUpTorch (triggerObject);
		}
	}

	void pickUpTorch(GameObject triggerObject){
		Debug.Log ("Torch has been picked up");
		transform.SetParent (triggerObject.transform.FindChild("Torch Holder"));
		transform.position = transform.parent.position;
		transform.rotation = transform.parent.rotation;

		equipped = true;
	}

	void dropTorch(){
		//Moet nog gemaakt worden.
	}
}
