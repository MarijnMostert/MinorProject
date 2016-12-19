using UnityEngine;
using System.Collections;

public class AllSpeeds : MonoBehaviour {

	public float allspeeds;
	public float allamps;

	public void Restart () {
		PingPongPlatform[] these = GetComponentsInChildren<PingPongPlatform> ();
		foreach (PingPongPlatform tmp in these) {
			tmp.maxSpeed = allspeeds;
			tmp.amplitude = allamps;
		}
	}
}
