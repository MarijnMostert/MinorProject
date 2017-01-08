using UnityEngine;
using System.Collections;

public class myLever : MonoBehaviour {

	public LeverActivator lever { get; set; }

	public void Reread () {
		lever = GetComponentInChildren<LeverActivator> ();
	}
}
