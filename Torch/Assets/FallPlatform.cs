using UnityEngine;
using System.Collections;

public class FallPlatform : MonoBehaviour {

	public float fallTime;
	public bool active = true;
	public float timeLeft;
	private bool entered = false;
	private bool movingback = false;
	private Vector3 startingpos;

	// Use this for initialization
	void Start () {
		if (active) {
			entered = GetComponentInChildren<PlatformTrigger> ().entered;
			timeLeft = fallTime;
			startingpos = transform.position;
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

			if (timeLeft < -3) {
				entered = false;
				GetComponentInChildren<PlatformTrigger> ().resetEntered ();
				timeLeft = fallTime;
				GetComponent<Rigidbody> ().isKinematic = true;
				movingback = true;
			}

			if (movingback) {
				transform.position = Vector3.MoveTowards (transform.position, startingpos, 0.7f);
				if (transform.position == startingpos) {
					movingback = false;
				}
			}
		}
	}

}
