using UnityEngine;
using System.Collections;

public class PowerUpSticky : MonoBehaviour, IPowerUp {


	public GameObject Sticky;

	public void Use(){
		GameObject sticky = Instantiate(Sticky, transform.parent.position, transform.parent.rotation) as GameObject;
		sticky.transform.position = new Vector3 (sticky.transform.position.x, 0f, sticky.transform.position.z);

	}
}
