using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuyableItem : MonoBehaviour {

	public string name;
	public Sprite icon;
	public int price;
	public bool owned;
	[HideInInspector] public ShopInterfaceButton shopInterfaceButton;
}
