using UnityEngine;
using System.Collections;

public class ResetHomeScreenPlayer : MonoBehaviour {

	public float minX, maxX, minY, maxY, minZ, maxZ;

	void OnEnable(){
		StartCoroutine (ResetPlayer ());
	}
	
	void OnDisable(){
		StopAllCoroutines ();
	}

	IEnumerator ResetPlayer(){
		while (true) {
			yield return new WaitForSeconds (2f);
			if (transform.position.x < minX || transform.position.x > maxX || transform.position.y < minY || transform.position.y > maxY) {
				if (transform.position.z < minZ || transform.position.z > maxZ) {
					RepositionPlayer ();
				}
			}
		}
	}

	void RepositionPlayer(){
		transform.position = new Vector3 (0f, 0f, 0f);
	}
}
