using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerWeaponController : WeaponController {

	public int playerNumber;
	public Inventory inventory;
	public GameObject indicator;

	private string attackButton;

	void Awake(){
	}

	new void Start () {
		base.Start ();
		//inventory = gameObject.GetComponent<Inventory> ();
//		attackButtonController = "ControllerAttack" + playerNumber;
	}

	public override void Equip(Weapon weapon){
		if (!inventory.weapons.Contains (weapon)) {
			base.Equip (weapon);
			inventory.weapons.Add (weapon);
			inventory.setIcon (weapon);
			indicator.transform.position = GameObject.Find ("WeaponIcon" + inventory.weapons.Count).transform.position;
		} else {
			Debug.Log ("Inventory already conntains " + weapon);
		}
	}
    
    public void setNumber(int playerNumber)
    {
        this.playerNumber = playerNumber;
        attackButton = "Attack" + playerNumber;
    }

	public void setIndicator(Color playerColor){
		indicator = Instantiate (indicator, GameObject.Find ("UI Inventory").transform) as GameObject;
		indicator.name = "Weapon Indicator P" + playerNumber;
		indicator.GetComponent<Image>().color = playerColor;
		indicator.transform.position = GameObject.Find ("WeaponIcon0").transform.position;
	}

    void Update () {
		if (Input.GetButton (attackButton)) {
			Attack ();
		}
	}

	private void Attack(){
		currentWeapon.Fire();
	}
}
