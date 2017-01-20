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
	public float turningVariable;
	public float timer;
	public Vector3 startPosition;
	public float sinusOffset = 0f;
	public bool firstRotation = false;
	private Vector3 smoothDampVar;
	public float smoothTime = 1f;
	private float savedRotation = 0f;

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

				/*
				if (Time.time - timer > .5f && savedRotation < 180f) {
					transform.Rotate (new Vector3 (0f, 10f, 0f));
					transform.Translate (Vector3.forward * moveDistance);
					savedRotation += 10f;
				} else if (Time.time - timer > .5f) {
					transform.position = Vector3.MoveTowards (transform.position, GameManager.Instance.playerManagers [0].playerInstance.transform.position, 0f);
				} else {
					transform.Translate (Vector3.forward * moveDistance);

				}
*/


				if (!firstRotation) {
					transform.Rotate (new Vector3 (0f, -90f, 0f));
					firstRotation = true;
				}

				float rotationFactor = 3 * (Mathf.Sin (10*(Time.time - timer) + sinusOffset) + 2);

				transform.Rotate (new Vector3 (0f, rotationFactor, 0f));
				transform.Translate (Vector3.forward * moveDistance);





				/*
				float length = 5;

				if (startPosition == new Vector3(0f,0f,0f)) {
					startPosition = transform.position - new Vector3(length, 0f, 0f);
				}


				float x = length - ((Time.time - timer)*3);

				if (x <= 0) {
					x = ((Time.time - timer) * 3) - length;
					transform.position = startPosition + new Vector3 (x, 0f, -1 * (Mathf.Sqrt (length * x) - x));

				} else {

					transform.position = startPosition + new Vector3 (x, 0f, (Mathf.Sqrt (length * x) - x));

				}

*/
				/*
				transform.Rotate (new Vector3 (0f, turningVariable, 0f));
				transform.Translate (Vector3.forward * moveDistance);
				*/








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
		if (Boomerang) {
			timer = Time.time;
			transform.Rotate (new Vector3 (0f, 90f, 0f));
			savedRotation = 0f;
		}
	}

	void DestroyProjectile(){
		if (enemyHit && PlayerData != null) {
			PlayerData.IncrementShotsLanded ();
		}
		if (TargetFinder) {
			Target = null;
		} else if (Boomerang) {
			firstRotation = false;
		}
		ToggleComponents (false);
		activated = false;
		startPosition = new Vector3 (0f, 0f, 0f);
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
