using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuyableItem : MonoBehaviour {

	public string name;
	public Sprite icon;
	public int price;
	public bool owned;
	public bool equipped;
	public bool multiEquippable = false;
	[HideInInspector] public ShopInterfaceButton shopInterfaceButton;
	/// <summary>
	/// The type.
	/// "Pet"
	/// "Torch"
	/// "Weapon Upgrade"
	/// "Health Upgrade"
	/// "Cape Skin"
	/// </summary>
	public string type;

	[Header ("If object:")]
	public GameObject equippableObject;
	[Header ("If damage multiplier upgrade:")]
	public float multiplier;
	[Header ("If max health upgrade:")]
	public int addedMaxHealth;
	[Header ("If skin:")]
	public Material skin;

	public void Equip(int index){
		GameManager.Instance.data.shopItemsEquipped [index] = true;

		switch (type) {
		case "Pet":
			GameManager.Instance.ChangePet (equippableObject);
			break;
		case "Torch":
			GameManager.Instance.ChangeTorch (equippableObject);
			break;
		case "Weapon Upgrade":
			GameManager.Instance.data.playerDamageMultiplier *= multiplier;
			break;
		case "Health Upgrade":
			GameManager.Instance.data.playerMaxHealth += addedMaxHealth;
			break;
		case "Cape Skin":
			GameManager.Instance.data.playerSkin [0] = skin;
			break;
		case "Hat Skin":
			GameManager.Instance.data.playerSkin [1] = skin;
			break;
		}
	}
}
