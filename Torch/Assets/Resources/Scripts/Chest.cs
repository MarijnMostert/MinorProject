using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Chest : InteractableItem {

	public GameObject[] contents;

	[Header ("- Flyout properties")]
	public float firingAngle = 45.0f;
	public float gravity = 9.8f;
	private bool used = false;

	public bool getUsed() {
		return used;
	}

	void Start(){
		base.Start ();

		for (int i = 0; i < contents.Length; i++) {
			contents[i] = Instantiate (contents [i]) as GameObject;
			contents[i].SetActive (false);
		}
	}

	public override void action(GameObject triggerObject){
		if (!used) {
			AudioSource audio = GetComponent<AudioSource> ();
			audio.pitch = Random.Range (0.8f, 1.1f);
			audio.Play ();
			GetComponent<Animator> ().SetTrigger ("Open Chest");
			for (int i = 0; i < contents.Length; i++) {
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

    public void addItem(GameObject item)
    {
        System.Array.Resize(ref contents,contents.Length+1);
        contents[contents.Length-1] = item;
    }

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
}