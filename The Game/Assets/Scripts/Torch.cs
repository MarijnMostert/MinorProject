using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Torch : InteractableItem, IDamagable {
	public int startingHealth;
	public Light torchLight;
	public float startingIntensity = 4f;
	[SyncVar(hook = "OnChangeHealth")]
	public int health;
	public float range = 5f;
	public float smoothingTime = 1f;
	[HideInInspector] //This variable will be used by other scripts but will not be editable in the Unity GUI.
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
	private Text healthText;
	private Text deathText;
	private string interactionButton = "InteractionButton";
	private bool releaseAllowed = false;

	void Awake(){
		torchLight = transform.GetComponentInChildren<Light> ();
		healthText = GameObject.Find ("Health Text").GetComponent<Text>();
		deathText = GameObject.Find("UI").transform.FindChild("Death Text").GetComponent<Text> ();
		if (healthText == null || deathText == null)
			Debug.Log ("Add UI Prefab to the scene");
	}

	public override void Start () {
		base.Start ();
		torchLight.intensity = startingIntensity;
		intensityBase = startingIntensity;
		randomFactorIntensity = startingIntensity / 8f;
		randomFactorRange = range / 8f;
		health = startingHealth;

		if(healthText != null)
			healthText.text = "Health: " + health;

		//GameObject torch = (GameObject)Instantiate(torchPrefab, transform.position, transform.rotation);
		//NetworkServer.Spawn (torch);

		//Every 'flickerInterval' seconds the 'torchFlickering()' function is called.
		InvokeRepeating ("torchFlickering", 0f, flickerInterval);
	}
	
	void Update () {
		lightUpdate ();
		StartCoroutine (waitOneFrame());//every frame restart counter
		if (releaseAllowed) {
			releaseTorch ();
		}
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

	//For when the torch takes damage
	public void takeDamage(int damage){
		health -= damage;
		//updateHealth ();

		if (isDead () && !dead) {
			onDead ();
		}
	}

	//For when the player e.g. picks up a healthPickUp.
	public void heal(int healingAmount){
		health += healingAmount;
		//updateHealth ();
	}
	//function to update syncvar health across network and update healthText
	void OnChangeHealth (int health){
		healthText.text = "Health: " + health;
	}
		
	//Random deviation from the base intensity and range.
	private void torchFlickering(){
		float randomValue = Random.value;
		torchLight.intensity = Mathf.SmoothDamp (torchLight.intensity, intensityBase + ((2f * randomValue) - 1f) * randomFactorIntensity, ref smoothDampVar3, flickerInterval);
		torchLight.range = Mathf.SmoothDamp (torchLight.range, rangeBase + ((2f * randomValue) - 1f) * randomFactorRange, ref smoothDampVar4, flickerInterval);
	}

	//Update the health of the torch.
	//private void updateHealth(){
	//	healthText.text = "Health: " + health;
	//}

	private void onDead(){
		health = 0;
		dead = true;
		CancelInvoke ();
		foreach  (GameObject player in GameObject.FindGameObjectsWithTag("Player") ){
		Destroy (player);
		}// destroy all player objects; not the parent of the torch
		//	transform.parent.gameObject.SetActive(false);
		//	gameObject.SetActive (false);
		deathText.gameObject.SetActive(true);
		GameObject.Find ("UI/Score Text").SetActive (false);
		GameObject.Find ("UI/Health Text").SetActive(false);
		GameObject.FindWithTag ("CursorPointer").SetActive (false);
	}

	public override void action(){
		Debug.Log ("into action()");
		if (Object.ReferenceEquals(transform.parent, null)) {// if not yet picked up
			GameObject player = GameObject.FindWithTag ("Player");
			transform.parent = player.transform;//Pickup
			print ("Picked up Torch");
		}
	}

	IEnumerator waitOneFrame(){
		yield return 0;
		GameObject player = GameObject.FindWithTag ("Player");
		if (Object.Equals(transform.parent, player.transform)) {//Torch with player condition
			releaseAllowed = true;// only able to release one frame after the frame where the torch was picked up
		}
	}

	void releaseTorch(){
		GameObject player = GameObject.FindWithTag ("Player");
		if (Object.Equals(transform.parent, player.transform)) {//Torch with player condition
			waitOneFrame();// only able to release one frame after the frame where the torch was picked up
			if (Input.GetButtonDown (interactionButton)) {
				transform.parent = null;//Release
				releaseAllowed = false;
			}
		}		
	}

}
