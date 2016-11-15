using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public int damage;
	public float lifeTime;

	private float speed;


	// Use this for initialization
	void Start () {
		Destroy (gameObject, lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.forward * Time.deltaTime * speed);
	}

	public void setSpeed(float newSpeed){
		speed = newSpeed;
	}
}
