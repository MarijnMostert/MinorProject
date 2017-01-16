using UnityEngine;
using System.Collections;

public class PowerUpShield : PowerUp, IPowerUp {

	public GameObject shield;

	override public void Use(){
		base.Use ();
		shield = Instantiate (shield, transform.parent.position, transform.parent.rotation, transform.parent) as GameObject;
		Destroy (gameObject);
	}
}
