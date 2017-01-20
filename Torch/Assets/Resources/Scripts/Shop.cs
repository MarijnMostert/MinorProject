using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

	private GameManager gameManager;
	private Data data;
	public Camera shopCam;
	[SerializeField] private ParticleSystem particles;
	public Transform itemPosition;
	public BuyableItem[] itemsToBuy;
	[SerializeField] private BuyableItem activeItem;
	private int activeIndex;
	[SerializeField] private Text coinsToSpend;
	[SerializeField] private GameObject BuyButton;
	[SerializeField] private GameObject EquipButton;
	public GameObject interfaceScreen;
	public GameObject InterfacePanelPrefab;

	public Text itemText;
	public Text descriptionText;

	void Start () {
		if (gameManager == null) {
			gameManager = GameManager.Instance;
		}

		UpdateCoinText ();

		for(int i = 0; i < itemsToBuy.Length; i++){
			itemsToBuy [i] = Instantiate (itemsToBuy [i], itemPosition.position, itemPosition.rotation, transform) as BuyableItem;
			itemsToBuy [i].gameObject.SetActive (false);
			itemsToBuy [i].owned = gameManager.data.shopItemsOwned [i];
			itemsToBuy [i].equipped = gameManager.data.shopItemsEquipped [i];

			ShopInterfaceButton shopInterfaceButton = CreateShopInterfaceButton (i);

			itemsToBuy [i].shopInterfaceButton = shopInterfaceButton;
				
			//interfacePanel.pivot = new Vector2 ((i % 2) * 20f, i * 20f);

		}

		//Set first item to active
		activeIndex = 1;
		SetItemActive (0);

		EquipActives ();
	}
		
	//Everytime the shop is set active, the available money text is being updated.
	void OnEnable(){
		if (gameManager == null) {
			gameManager = GameManager.Instance;
		}
		UpdateCoinText ();
	}

	ShopInterfaceButton CreateShopInterfaceButton (int index)
	{
		GameObject interfacePanel = Instantiate (InterfacePanelPrefab, new Vector3 (0f, 0f, 0f), Quaternion.identity, interfaceScreen.transform) as GameObject;
		interfacePanel.transform.position = interfaceScreen.transform.position;
		interfacePanel.transform.localRotation = Quaternion.Euler (new Vector3 (0f, 0f, 0f));
		float x = -160f + 150f * (index % 3);
		int yFactor = (int)(index / 3);
		float y = -85f + yFactor * -150;
		interfacePanel.transform.localPosition = interfacePanel.transform.localPosition + new Vector3 (x, y, 0f);
		ShopInterfaceButton shopInterfaceButton = interfacePanel.GetComponent<ShopInterfaceButton> ();
		shopInterfaceButton.Setup (this, index);
		return shopInterfaceButton;
	}

	//Method is going to be called by UI to show a preview of the item.
	public void SetItemActive(int itemNumber){
		if (activeIndex != itemNumber) {
			itemsToBuy[activeIndex].gameObject.SetActive (false);
			activeIndex = itemNumber;
			itemsToBuy[activeIndex].gameObject.SetActive (true);
			particles.Stop ();
			particles.Play ();

			BuyButton.SetActive (!gameManager.data.shopItemsOwned[itemNumber]);
			SetBuyButtonText ();
			EquipButton.SetActive (gameManager.data.shopItemsOwned [itemNumber]);

			itemText.text = itemsToBuy[activeIndex].itemName;
			descriptionText.text = itemsToBuy [activeIndex].itemDescription;
		}
	}

	//For updating the text field in the UI of the shop
	void UpdateCoinText(){
		if (coinsToSpend != null) {
			coinsToSpend.text = "Coins: " + GameManager.Instance.data.coins.ToString ();
		}
	}

	//This method is probably gonna used by a button in the UI of the shop.
	public void BuyItem(){
		if (gameManager.data.coins >= itemsToBuy[activeIndex].price && !gameManager.data.shopItemsOwned[activeIndex]) {
			gameManager.data.coins -= itemsToBuy[activeIndex].price;
			UpdateCoinText ();

			//Set Equip button to true after purchasing
			BuyButton.SetActive (false);
			SetBuyButtonText ();
			EquipButton.SetActive (true);

			SetOwned (activeIndex);

			gameManager.achievements.firstBoughtAchievement ();
		}
	}

	public void EquipItem(){

		//Dequip all other items of the same type (if multi-equippable is disabled)
		string type = itemsToBuy[activeIndex].type;
		for (int i = 0; i < itemsToBuy.Length; i++) {
			if (itemsToBuy [i].type.Equals(type) && !itemsToBuy[i].multiEquippable) {
				if (gameManager.data.shopItemsOwned [i]) {
					itemsToBuy [i].shopInterfaceButton.setOwned ();
					gameManager.data.shopItemsEquipped [i] = false;
				}
			}
		}

		//Set current item equipped
		itemsToBuy[activeIndex].shopInterfaceButton.setEquipped ();
		gameManager.data.shopItemsEquipped [activeIndex] = true;
		itemsToBuy[activeIndex].Equip (activeIndex);
	}

	public void SetOwned(int index){
		ShopInterfaceButton shopInterfaceButton = itemsToBuy [index].shopInterfaceButton;
		shopInterfaceButton.setOwned ();
		gameManager.data.shopItemsOwned [index] = true;

		checkAllOwned ();
	}

	private void checkAllOwned () {
		if (!gameManager.achievements.all_shopitems_bought) {
			bool allOwned = true;
			for (int i = 0; i < itemsToBuy.Length; i++) {
				Debug.Log (gameManager.data.shopItemsOwned [i]);
				if (!gameManager.data.shopItemsOwned [i])
					allOwned = false;
			}
			Debug.Log ("================= Ride");

			if (allOwned) {
				gameManager.achievements.shopAchievement ();
			}
		}
	}

	public void ToggleShop(){
		gameManager.ToggleShop ();
	}

	public void EquipActives(){
		for (int i = 0; i < itemsToBuy.Length; i++) {
			if (GameManager.Instance.data.shopItemsEquipped[i]) {
				itemsToBuy [i].Equip (i);
			}
		}
	}

	public void SetBuyButtonText(){
		BuyButton.GetComponentInChildren<Text>().text = "BUY\nCOST: " + itemsToBuy [activeIndex].price;
	}
}
