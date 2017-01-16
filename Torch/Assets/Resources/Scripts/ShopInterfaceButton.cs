using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopInterfaceButton : MonoBehaviour {

	[HideInInspector] public BuyableItem item;
	[HideInInspector] public int index;
	[HideInInspector] public Shop shop;
	public Text itemText;
	public Image itemImage;
	public Image coinImage;
	public Text priceText;
	public Text ownedText;
	public Text equippedText;

	public void Setup(Shop shop, int itemIndex){
		this.shop = shop;
		this.item = shop.itemsToBuy [itemIndex];
		this.index = itemIndex;
		this.itemText.text = this.item.name;
		this.itemImage.sprite = this.item.icon;
		this.priceText.text = this.item.price.ToString ();
		bool owned = this.item.owned;
		bool equipped = this.item.equipped;

		//Set correct objects active.
		priceText.gameObject.SetActive (!owned);
		coinImage.gameObject.SetActive (!owned);
		ownedText.gameObject.SetActive (owned && !equipped);
		equippedText.gameObject.SetActive (owned && equipped);
		//equippedText.gameObject.SetActive (owned);
	}

	public void Click(){
		shop.SetItemActive (index);
	}

	public void setOwned(){
		priceText.gameObject.SetActive (false);
		coinImage.gameObject.SetActive (false);
		ownedText.gameObject.SetActive (true);
		equippedText.gameObject.SetActive (false);
	}

	public void setEquipped(){
		priceText.gameObject.SetActive (false);
		coinImage.gameObject.SetActive (false);
		ownedText.gameObject.SetActive (false);
		equippedText.gameObject.SetActive (true);
	}
}
