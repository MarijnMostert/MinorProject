using UnityEngine;
using System.Collections;

public class PowerUpSticky : MonoBehaviour, IPowerUp {

	public GameObject Sticky;
	public float spawnHeight = -1.5f;

	public void Use(){
		GameObject sticky = Instantiate(Sticky, transform.parent.position, transform.parent.rotation, GameManager.Instance.levelTransform) as GameObject;
		sticky.transform.position = new Vector3 (sticky.transform.position.x, spawnHeight, sticky.transform.position.z);

	}
}
