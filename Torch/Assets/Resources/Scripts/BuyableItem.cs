using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuyableItem : MonoBehaviour {

	public string name;
	public Sprite icon;
	public int price;
	public bool owned;
	public bool equipped;
	[HideInInspector] public ShopInterfaceButton shopInterfaceButton;
	public int type;
	public GameObject equippableObject;

	/*
	 * Type 0 = Pet
	 * Type 1 = Torch
	 * 
	 * 
	 * 
	 * 
	 * 
	 * 
	 * 
	 */

	public void Equip(){
		if (type == 0) {
			GameManager.Instance.ChangePet (equippableObject);
		} else if (type == 1) {
			GameManager.Instance.ChangeTorch (equippableObject);
		}
	}
}
