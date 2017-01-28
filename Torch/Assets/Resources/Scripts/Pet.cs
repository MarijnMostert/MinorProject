using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class Pet : AudioObject {

	public GameObject speechCanvas;
	public Image speechImage;
	public Text speechText;

	public AudioClip[] clips;
	public float chanceForClip = 0.01f;
	public float interval = 5f;
	public bool attacking = true;
	public WeaponController weaponController;
	public TargetFinder targetFinder;
	public GameObject target;
	public float timeOut = 3f;
	public Animator animator;
	private Vector3 prevPos;

	void Start(){
		speechImage.gameObject.SetActive (false);
		speechText.text =  "";
		if (clips != null) {
			StartCoroutine (RandomSayings ());
		}
		weaponController = GetComponent<WeaponController> ();
		StartCoroutine (Attack ());

	}

	void Update(){
		if (animator != null) {
			float velocity = (transform.position - prevPos).sqrMagnitude;
			Debug.Log (velocity);
			animator.SetFloat ("Velocity", velocity);
			prevPos = transform.position;
		}
	}

	IEnumerator RandomSayings(){
		while (true) {
			if (Random.value < chanceForClip) {
				AudioClip clip = clips [Random.Range (0, clips.Length)];
				if (clip != null) {
					ObjectPooler.Instance.PlayAudioSource (clip, mixerGroup, pitchMin, pitchMax, transform);
				}
			}
			yield return new WaitForSeconds (interval);
		}
	}

	void OnDestroy(){
		StopAllCoroutines ();
	}

	IEnumerator Attack(){
		while(attacking){
			yield return new WaitForSeconds (timeOut);

			if (weaponController == null) {
				weaponController = GetComponentInChildren<WeaponController> ();
			}

			targetFinder.FindTargets ();
			if ((target == null || !target.activeInHierarchy) && targetFinder.targets.Count != 0) {
				target = targetFinder.targets [Random.Range(0, targetFinder.targets.Count)];
			}

			if (target != null) {
				if (target.activeInHierarchy) {
					//Debug.Log (target);
					weaponController.transform.LookAt (target.transform);
					weaponController.Fire ();
					target = null;
				}
			}
		}
	}

}
