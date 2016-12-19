using UnityEngine;
using System.Collections;

public class FallPlatform : MonoBehaviour {

	public float fallTime;
	private float timeLeft;
	private bool entered = false;

	// Use this for initialization
	void Start () {
		entered = GetComponentInChildren<PlatformTrigger>().entered;
		timeLeft = fallTime;
	}
	
	// Update is called once per frame
	void Update () {
		entered = GetComponentInChildren<PlatformTrigger>().entered;
		if (entered) {
			timeLeft -= Time.deltaTime;
		}

		if (timeLeft < 0) {
			GetComponent<Rigidbody> ().isKinematic = false;
		}
	}
}
