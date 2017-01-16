using UnityEngine;
using System.Collections;

public class PowerUpDecoy: PowerUp, IPowerUp {

	public GameObject Decoy;
	public float spawnHeight = -1.5f;

	override public void Use(){
		base.Use ();
		GameObject decoy = Instantiate(Decoy, transform.parent.position + transform.parent.forward * 2, transform.parent.rotation, GameManager.Instance.levelTransform) as GameObject;
		Vector3 temp = decoy.transform.position;
		temp.y = spawnHeight;
		decoy.transform.position = temp;
	}
}
