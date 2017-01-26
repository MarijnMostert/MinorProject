using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Projectile : AudioObject {

	public int ObjectPoolerIndex;
	public bool hasParticlesOnHit = false;
	public int ParticlesPoolerIndex;
	private GameObject particlesOnHit;
	public AudioClip clipOnImpact;
	public int minDamage;
	public int maxDamage;
	public float critChance = 0.05f;
	public float lifeTime;
	public LayerMask collisionMask;
	public float speed;
	public bool piercing = false;
	private List<Enemy> enemiesHit;
	private bool activated = true;
	[HideInInspector] public PlayerData PlayerData;
	private bool enemyHit = false;
	[HideInInspector] public float multiplier = 1;

	[Header ("- If rocket:")]
	public bool TargetFinder;
	public Collider col;
	public GameObject Target;

	[Header ("- If Boomerang:")]
	public bool Boomerang;
	private bool initialized = false;
	private bool targetReached = false;
	private Vector3 boomerangTarget;
	[HideInInspector] public RangedWeapon rangedWeapon;

	[Serializable]
	public struct ComponentsToToggle {
		public float timeOut;
		public TrailRenderer trailRenderer;
		public ParticleSystem particles;
		public MeshRenderer meshRenderer;
		public Collider col;
		public Light light;
	}
	public ComponentsToToggle comp;

	void OnEnable () {
		if (piercing) {
			enemiesHit = new List<Enemy> ();
		}
		ResetProjectile ();
		//Set the bullet inactive after lifeTime seconds
		StartCoroutine (SetInactive ());
	}

	IEnumerator SetInactive(){
		yield return new WaitForSeconds (lifeTime);
		if (gameObject.activeInHierarchy)
			DestroyProjectile ();
	}

	void FixedUpdate () {
		if (activated) {
			float moveDistance = speed * Time.deltaTime;
			checkCollisions (moveDistance);
			if (TargetFinder) { //Rocket projectile
				if (Target != null) {
					transform.LookAt (Target.transform);
				}
				transform.Translate (Vector3.forward * moveDistance);
			} else if (Boomerang) { //Boomerang projectile
				if (!initialized) {
					Vector3 cursorPointer = PlayerData.GetComponent<PlayerMovement>().GetCursorPointer().transform.position;
					boomerangTarget = new Vector3 (cursorPointer.x, transform.position.y, cursorPointer.z);
					transform.LookAt (boomerangTarget);
					initialized = true;
					targetReached = false;
				}

				if (!targetReached && (transform.position - boomerangTarget).magnitude > .5f) {
					transform.Translate (Vector3.forward * moveDistance);
				} else if ((transform.position - PlayerData.transform.position).magnitude > .2f) {
					targetReached = true;
					transform.LookAt (PlayerData.transform);
					transform.Translate (Vector3.forward * moveDistance);
				} else {
					DestroyProjectile ();
				}
			} else { //Normal projectile
				transform.Translate (Vector3.forward * moveDistance);
			}
		}
	}

	public void setSpeed(float newSpeed){
		speed = newSpeed;
	}

	private void checkCollisions(float moveDistance){
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide)) {
			onHitObject (hit);
		}

	}

	private void onHitObject(RaycastHit hit){
		IDamagable damagableObject = hit.collider.GetComponent<IDamagable> ();
		GameObject objectHitted = hit.collider.gameObject;

		if (damagableObject != null) {
			if (piercing && enemiesHit.Contains(objectHitted.GetComponent<Enemy>())){
				return;
			}
			int damage = UnityEngine.Random.Range ((int)(minDamage * multiplier), (int)(maxDamage * multiplier));
			bool crit = false;
			if (UnityEngine.Random.value < critChance) {
				damage *= 2;
				crit = true;
			}

			enemyHit = true;
			damagableObject.takeDamage (damage, crit, gameObject);
			//Debug.Log ("hit " + damagableObject);
			}

		if (objectHitted.CompareTag ("Shield")) {
			return;
		}
			
		if(objectHitted.CompareTag ("Target")) {
			objectHitted.GetComponent<RotateTarget> ().Rotate (transform.forward);
		}

		if (hasParticlesOnHit) {
			particlesOnHit = ObjectPooler.Instance.GetObject (ParticlesPoolerIndex, true, hit.point, 
				Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0f,360f), UnityEngine.Random.Range(0f,360f), UnityEngine.Random.Range(0f,360f))));
		}
			
		if (piercing && objectHitted.CompareTag ("Enemy")) {
			enemiesHit.Add(objectHitted.GetComponent<Enemy>());
			return;
		}

		if(clipOnImpact != null)
			ObjectPooler.Instance.PlayAudioSource (clipOnImpact, mixerGroup, pitchMin, pitchMax, transform);

		DestroyProjectile();
	}

	void ResetProjectile(){
		ToggleComponents (true);
		activated = true;
		initialized = false;
	}

	void DestroyProjectile(){
		if (rangedWeapon != null && rangedWeapon.maxAmountOfBulletsAlive) {
			rangedWeapon.DecrementBulletsAlive ();
		}
		if (enemyHit && PlayerData != null) {
			PlayerData.IncrementShotsLanded ();
		}
		if (TargetFinder) {
			Target = null;
		}
		ToggleComponents (false);
		activated = false;
		StopAllCoroutines ();
		StartCoroutine (KillProjectile ());
	}

	IEnumerator KillProjectile(){
		yield return new WaitForSeconds (comp.timeOut);
		gameObject.SetActive (false);
	}

	void ToggleComponents(bool enable){
		if (comp.col != null)
			comp.col.enabled = enable;
		if (comp.trailRenderer != null)
			comp.trailRenderer.enabled = enable;
		if (comp.meshRenderer != null)
			comp.meshRenderer.enabled = enable;
		if (comp.particles != null) {
			if(!enable){
				comp.particles.Stop ();
			}
			else{
				comp.particles.Play();
			}
		}
		if (comp.light != null) {
			comp.light.enabled = enable;
		}
	}
}
