using UnityEngine;
using System.Collections;

public class WallReplacer : MonoBehaviour {
	public float timeMovement;
	public float replacement;
	private int count = 0;
	
	// Update is called once per frame
	void Update () {
		Vector3 currentPos = transform.position;
		if (Time.time > timeMovement && count == 0){
			transform.position = currentPos + new Vector3(replacement, 0.0f, 0.0f);
			count = 1;
		}
	}
}
