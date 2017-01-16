using UnityEngine;
using System.Collections;

public class Laserbeam : MonoBehaviour {

	public float speed;
	public float maxLength;
	public int damage;
	public float cooldown;
	private float timestamp = 0f;
	public bool active = true;

	Transform myTransform;
	LineRenderer lineRenderer;
	Vector3 laserVector;
	RaycastHit hit;

	void Start() {
		myTransform = transform;
		lineRenderer = (LineRenderer) GetComponent<LineRenderer>();
		lineRenderer.SetPosition (1, new Vector3 (0, 0, maxLength));

		int number = Random.Range (0, 360);
		myTransform.Rotate (0, number, 0);
	}

	void Update() {
		if (active) {
			myTransform.Rotate (0, speed*Time.deltaTime/Time.fixedDeltaTime, 0);
			UpdateLength ();
		}
	}

	void UpdateLength(){
		if (Physics.Raycast (myTransform.position, myTransform.forward, out hit) && Time.time - timestamp > cooldown) {
			lineRenderer.SetPosition (1, new Vector3 (0, 0, hit.distance));

			if (hit.collider.gameObject.CompareTag("Player")) {
				hit.collider.gameObject.GetComponent<IDamagable> ().takeDamage (damage, false, gameObject);
				timestamp = Time.time;
			}
		} else
			lineRenderer.SetPosition (1, new Vector3 (0, 0, maxLength));
	}
}