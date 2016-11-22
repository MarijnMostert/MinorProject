using UnityEngine;
using System.Collections;

public class WallMover : MonoBehaviour {

	public bool start = false;
	public float wallSpeed;
	public float DestroyWhenZ;
	public int Damage;
	private GameObject target;

	//If wall collides with player, decrease health
	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("Player")) {
			target.GetComponent<Torch> ().takeDamage (Damage);
		}
	}

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Torch");
	}

	// Update is called once per frame
	void Update () {
		if (start == true) {
			Vector3 Position = transform.position;
			Vector3 movement = new Vector3 (wallSpeed, 0.0f, 0.0f);
			transform.Translate (movement * Time.deltaTime);

			if (Position.z < DestroyWhenZ) {
				GameObject.Find ("MonsterPlatforms").GetComponent<MonsterPlatformMover> ().start = true;
				Destroy (gameObject);
			}
		}
	}

}
