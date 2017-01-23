using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class Chest : InteractableItem {

	public bool manualChest;
	public List<GameObject> contents;
	public AudioClip clip;
	public Animator animator;

	[Header ("- Flyout properties")]
	public float firingAngle = 45.0f;
	public float gravity = 9.8f;
	private bool used = false;

	public bool getUsed() {
		return used;
	}

	public override void Start(){
		base.Start ();

		if (manualChest) {
			InstantiateContent ();
		}
	}

	public override void action(GameObject triggerObject){
		if (!used) {
			ObjectPooler.Instance.PlayAudioSource (clip, mixerGroup, pitchMin, pitchMax, transform);
			animator.SetTrigger ("Open Chest");
			for (int i = 0; i < contents.Count; i++) {
				flyOut (contents [i]);
			}
			used = true;
			Destroy (gameObject, 1.5f);
		}
	}

	void flyOut(GameObject obj){
		float randomX = Random.Range (-2f, 2f);
		float randomZ = Random.Range (-2f, 2f);
		Vector3 spawnLocation = new Vector3(transform.position.x + randomX, 0f, transform.position.z + randomZ);
		obj.SetActive (true);
		StartCoroutine (SimulateProjectile (spawnLocation, obj));
	}

	/*
    public void addItem(GameObject item)
    {
        System.Array.Resize(ref contents,contents.Count+1);
        contents[contents.Count-1] = item;
    }
    */

	IEnumerator SimulateProjectile(Vector3 Target, GameObject Projectile)
	{
		// Short delay added before Projectile is thrown
		//yield return new WaitForSeconds(1.5f);
		TogglePickUpScripts (Projectile, false);
		foreach (Collider col in Projectile.GetComponentsInChildren<Collider> ()) {
			col.enabled = false;
		}

		// Move projectile to the position of throwing object + add some offset if needed.
		Projectile.transform.position = transform.position + new Vector3(0, 0.0f, 0);

		// Calculate distance to target
		float target_Distance = Vector3.Distance(Projectile.transform.position, Target);

		// Calculate the velocity needed to throw the object to the target at specified angle.
		float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

		// Extract the X  Y componenent of the velocity
		float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
		float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

		// Calculate flight time.
		float flightDuration = target_Distance / Vx;

		// Rotate projectile to face the target.
		Projectile.transform.rotation = Quaternion.LookRotation(Target - Projectile.transform.position);

		float elapse_time = 0;

		while (elapse_time < flightDuration)
		{
			if (Projectile == null) {
				yield return null;
			} else {
				Projectile.transform.Translate (0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

				elapse_time += Time.deltaTime;

				yield return null;
			}
		}
		TogglePickUpScripts (Projectile, true);
		//Projectile.GetComponent<Collider> ().enabled = true;
		foreach (Collider col in Projectile.GetComponentsInChildren<Collider> ()) {
			col.enabled = true;
		}
	}  

	static void TogglePickUpScripts (GameObject Object, bool boolean)
	{
		if (Object.GetComponent<Hover> () != null)
			Object.GetComponent<Hover> ().enabled = boolean;
		if (Object.GetComponent<Rotate> () != null)
			Object.GetComponent<Rotate> ().enabled = boolean;
		if (Object.GetComponent<HealthPickUp> () != null)
			Object.GetComponent<HealthPickUp> ().enabled = boolean;
		if (Object.GetComponent<PowerUpPickUp> () != null)
			Object.GetComponent<PowerUpPickUp> ().enabled = boolean;
		if (Object.GetComponent<RangedWeaponPickUp> () != null)
			Object.GetComponent<RangedWeaponPickUp> ().enabled = boolean;
		if (Object.GetComponent<ScorePickUp> () != null)
			Object.GetComponent<ScorePickUp> ().enabled = boolean;
	}

	public void FillChest(DungeonData.DungeonParameters dungeonParameters){
		List<GameObject> temp = contents;
		contents = new List<GameObject> ();

		//coins
		for (int i = 0; i < Random.Range(1,4); i++) {
			contents.Add (temp [0]);
		}

		//health pickup
		for (int i = 0; i < Random.Range (1, 3); i++) {
			contents.Add (temp [1]);
		}

		//Powerups
		if (dungeonParameters.powerUps.shield.enabled && Random.value < dungeonParameters.powerUps.shield.spawnChance)
			contents.Add (temp [2]);
		if (dungeonParameters.powerUps.sticky.enabled && Random.value < dungeonParameters.powerUps.sticky.spawnChance)
			contents.Add (temp [3]);
		if (dungeonParameters.powerUps.wall.enabled && Random.value < dungeonParameters.powerUps.wall.spawnChance)
			contents.Add (temp [4]);
		if (dungeonParameters.powerUps.bomb.enabled && Random.value < dungeonParameters.powerUps.bomb.spawnChance)
			contents.Add (temp [5]);
		if (dungeonParameters.powerUps.decoy.enabled && Random.value < dungeonParameters.powerUps.decoy.spawnChance)
			contents.Add (temp [6]);

		//Weapons
		if (dungeonParameters.powerUps.iceballWeapon.enabled && Random.value < dungeonParameters.powerUps.iceballWeapon.spawnChance)
			contents.Add (temp [7]);
		if (dungeonParameters.powerUps.piercingWeapon.enabled && Random.value < dungeonParameters.powerUps.piercingWeapon.spawnChance)
			contents.Add (temp [8]);
		if (dungeonParameters.powerUps.laserWeapon.enabled && Random.value < dungeonParameters.powerUps.laserWeapon.spawnChance)
			contents.Add (temp [9]);
		if (dungeonParameters.powerUps.bloodWeapon.enabled && Random.value < dungeonParameters.powerUps.bloodWeapon.spawnChance)
			contents.Add (temp [10]);
		if (dungeonParameters.powerUps.boomerangWeapon.enabled && Random.value < dungeonParameters.powerUps.boomerangWeapon.spawnChance)
			contents.Add (temp [11]);
	}

	public void InstantiateContent(){
		for (int i = 0; i < contents.Count; i++) {
			contents [i] = Instantiate (contents [i], transform.position, Quaternion.identity) as GameObject;
			contents [i].SetActive (false);
		}
	}

	public void InstantiateContent(Transform parent){
		for (int i = 0; i < contents.Count; i++) {
			contents [i] = Instantiate (contents [i], transform.position, Quaternion.identity, parent) as GameObject;
			contents [i].SetActive (false);
		}
	}

	public void SetUp(DungeonData.DungeonParameters dungeonParameters, Transform parent){
		FillChest (dungeonParameters);
		InstantiateContent (parent);
	}
}