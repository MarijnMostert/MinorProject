using UnityEngine;
using System.Collections;

public class Laserbeam : MonoBehaviour {

	public float speed;
	Transform myTransform;
	public float maxLength;
	LineRenderer lineRenderer;
	Vector3 laserVector;
	RaycastHit hit;

	void Start() {
		myTransform = transform;
		lineRenderer = (LineRenderer) GetComponent<LineRenderer>();
		lineRenderer.SetPosition (1, new Vector3 (0, 0, maxLength));
	}

	void Update() {
		myTransform.Rotate (0, speed, 0);
		UpdateLength ();
	}

	void UpdateLength(){
		if (Physics.Raycast (myTransform.position, myTransform.forward, out hit)) {
			lineRenderer.SetPosition (1, new Vector3 (0, 0, hit.distance));

			if (hit.collider.gameObject.CompareTag("Player")) {
				KillPlayer ();
			}
		} else
			lineRenderer.SetPosition (1, new Vector3 (0, 0, maxLength));
	}

	void KillPlayer () {
		Torch torch = hit.collider.gameObject.GetComponentInChildren <Torch> ();
		int currentH = torch.health;
		torch.takeDamage (currentH);
	}

}









