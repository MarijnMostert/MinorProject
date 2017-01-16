using UnityEngine;
using System.Collections;

public class PowerUpWall : PowerUp, IPowerUp {

	public GameObject wall;

	override public void Use(){
		base.Use ();
		wall = Instantiate (wall, (transform.parent.position + transform.parent.forward * 2), transform.parent.rotation, GameManager.Instance.levelTransform) as GameObject;
		Destroy (gameObject);
	}
}
