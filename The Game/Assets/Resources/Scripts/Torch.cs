using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Torch : MonoBehaviour, IDamagable {

	public int startingHealth;
	public Light torchLight;
	public float startingIntensity = 4f;
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
<<<<<<< HEAD:The Game/Assets/Resources/Scripts/Torch.cs
	private Text deathText;

	void Awake(){
		torchLight = transform.GetComponentInChildren<Light> ();
		healthText = GameObject.Find ("Health Text").GetComponent<Text>();
		deathText = GameObject.Find("UI").transform.FindChild("Death Text").GetComponent<Text> ();
		if (healthText == null || deathText == null)
			Debug.Log ("Add UI Prefab to the scene");
	}
=======

	public GameManager gameManager;
	public bool equipped = false;
	public bool isDamagable = true;

	new void Start () {
		base.Start ();

		torchLight = transform.GetComponentInChildren<Light> ();
>>>>>>> master:Torch/Assets/Resources/Scripts/Torch.cs

	void Start () {
		torchLight.intensity = startingIntensity;
		intensityBase = startingIntensity;
		randomFactorIntensity = startingIntensity / 8f;
		randomFactorRange = range / 8f;

<<<<<<< HEAD:The Game/Assets/Resources/Scripts/Torch.cs
		health = startingHealth;

		if(healthText != null)
			healthText.text = "Health: " + health;
=======
		canvas.SetActive (true);
>>>>>>> master:Torch/Assets/Resources/Scripts/Torch.cs

		//Every 'flickerInterval' seconds the 'torchFlickering()' function is called.
		InvokeRepeating ("torchFlickering", 0f, flickerInterval);
	}
	
	void Update () {
		lightUpdate ();
		if (Input.GetButtonDown("DropTorch1") && equipped) {
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

	/**
	 * For when the torch takes damage
	 */
	public void takeDamage(int damage){
<<<<<<< HEAD:The Game/Assets/Resources/Scripts/Torch.cs
		health -= damage;
		updateHealth ();

		if (isDead () && !dead) {
			onDead ();
=======
		if (isDamagable) {
//		Debug.Log (gameObject + " takes " + damage + " damage.");
			health -= damage;
			updateHealth ();

			if (health <= 0) {
				Die ();
			}
>>>>>>> master:Torch/Assets/Resources/Scripts/Torch.cs
		}
	}

	//For when the player e.g. picks up a healthPickUp.
	public void heal(int healingAmount){
		health += healingAmount;
		if (health > gameManager.torchHealthMax) {
			health = gameManager.torchHealthMax;
		}
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
		if (healthText == null) {
			healthText = UI.transform.Find ("Health Text").GetComponent<Text> ();
		}
		healthText.text = "Health: " + health;
	}

	private void onDead(){
		health = 0;
<<<<<<< HEAD:The Game/Assets/Resources/Scripts/Torch.cs
		dead = true;
		CancelInvoke ();
		Destroy (transform.parent.gameObject);
		//	transform.parent.gameObject.SetActive(false);
		//	gameObject.SetActive (false);
		deathText.gameObject.SetActive(true);
		GameObject.Find ("UI/Score Text").SetActive (false);
		GameObject.Find ("UI/Health Text").SetActive(false);
		GameObject.FindWithTag ("CursorPointer").SetActive (false);

=======
		Destroy (canvas);
		Destroy (GameObject.FindGameObjectWithTag("CursorPointer"));
		gameManager.GameOver();
	}

	public override void action(GameObject triggerObject){
		pickUpTorch (triggerObject);
	}

	void pickUpTorch(GameObject triggerObject){
		Debug.Log ("Torch is picked up");
		transform.SetParent (triggerObject.transform.FindChild("Torch Holder"));
		transform.position = transform.parent.position;
		transform.rotation = transform.parent.rotation;
		gameManager.enemyTarget = triggerObject;
		gameManager.camTarget = triggerObject;
		canvas.SetActive (false);
		equipped = true;
	}

	void releaseTorch(){
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

	/*void InitializeLinkWithUI(){
		UI = GameObject.Find ("UI");
		if (UI != null) {
			healthText = UI.transform.FindChild ("Health Text").GetComponent<Text> ();
			healthText.text = "Health: " + health;
		}
>>>>>>> master:Torch/Assets/Resources/Scripts/Torch.cs
	}
	*/
}
