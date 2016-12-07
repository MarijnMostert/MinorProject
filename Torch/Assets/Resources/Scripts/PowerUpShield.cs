using UnityEngine;
using System.Collections;

public class PowerUpShield : MonoBehaviour, IPowerUp {

	public GameObject shield;

	public void Use(){
		shield = Instantiate (shield, transform.parent.position, transform.parent.rotation, transform.parent) as GameObject;
		Destroy (gameObject);
	}
}
