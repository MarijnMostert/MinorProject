using UnityEngine;
using System.Collections;

public class PowerUpWall : MonoBehaviour, IPowerUp {

	public GameObject wall;

	public void Use(){
		wall = Instantiate (wall, (transform.parent.position + transform.parent.forward * 2), transform.parent.rotation) as GameObject;
		Destroy (gameObject);
	}
}
