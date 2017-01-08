using UnityEngine;
using System.Collections;

public class FallPlatform : MonoBehaviour {

	public float fallTime;
	public bool active = true;
	private float timeLeft;
	private bool entered = false;

	// Use this for initialization
	void Start () {
		if (active) {
			entered = GetComponentInChildren<PlatformTrigger> ().entered;
			timeLeft = fallTime;
		}
	}

	// Update is called once per frame
	void Update () {
		if (active) {			
			entered = GetComponentInChildren<PlatformTrigger> ().entered;
			if (entered) {
				timeLeft -= Time.deltaTime;
			}

			if (timeLeft < 0) {
				GetComponent<Rigidbody> ().isKinematic = false;
			}
		}
	}
}
