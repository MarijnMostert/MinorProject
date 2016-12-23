using UnityEngine;
using System.Collections;

public class myLever : MonoBehaviour {

	public LeverActivator lever;

	public void Reread () {
		lever = GetComponentInChildren<LeverActivator> ();
	}
}
