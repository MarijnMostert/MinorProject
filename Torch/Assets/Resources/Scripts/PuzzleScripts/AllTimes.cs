using UnityEngine;
using System.Collections;

public class AllTimes : MonoBehaviour {

	public float alltimes;

	public void Restart () {
		FallPlatform[] these = GetComponentsInChildren<FallPlatform> ();
		foreach (FallPlatform tmp in these) {
			tmp.fallTime = alltimes;
		}
	}
}
