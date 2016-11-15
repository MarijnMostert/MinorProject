using UnityEngine;
using System.Collections;

public class PlayerWeaponController : MonoBehaviour {

	public int playerNumber;
	public RangedWeapon weapon;

	private string fireButton;

	// Use this for initialization
	void Start () {
		fireButton = "Fire" + playerNumber;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton (fireButton)) {
			weapon.fire ();
		}
	}
}
