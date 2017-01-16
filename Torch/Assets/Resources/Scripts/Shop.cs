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
	[SerializeField] private Text coinsToSpend;
	[SerializeField] private GameObject BuyButton;
	[SerializeField] private GameObject EquipButton;
	public GameObject interfaceScreen;
	public GameObject InterfacePanelPrefab;

	public Text itemText;

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

			GameObject interfacePanel = Instantiate (InterfacePanelPrefab, new Vector3(0f,0f,0f), Quaternion.identity, interfaceScreen.transform) as GameObject;
			interfacePanel.transform.position = interfaceScreen.transform.position;
			interfacePanel.transform.localRotation = Quaternion.Euler (new Vector3 (0f, 0f, 0f));
			float x = -160f + 150f * (i % 3);
			int yFactor = (int)(i / 3);
			float y = 170f + yFactor * -150;
			interfacePanel.transform.localPosition = interfacePanel.transform.localPosition + new Vector3 (x, y, 0f);
			ShopInterfaceButton shopInterfaceButton = interfacePanel.GetComponent<ShopInterfaceButton> ();
			shopInterfaceButton.Setup (this, i);

			itemsToBuy [i].shopInterfaceButton = shopInterfaceButton;
				
			//interfacePanel.pivot = new Vector2 ((i % 2) * 20f, i * 20f);

		}

		//Set first item to active
		activeItem = itemsToBuy [0];
		activeItem.gameObject.SetActive (true);

		//Set the buybutton active on default and the equipbutton inactive.
		BuyButton.SetActive (!activeItem.owned);
		EquipButton.SetActive (activeItem.owned);

		EquipActives ();
	}
		
	//Everytime the shop is set active, the available money text is being updated.
	void OnEnable(){
		if (gameManager == null) {
			gameManager = GameManager.Instance;
		}
		UpdateCoinText ();
	}

	//Method is going to be called by UI to show a preview of the item.
	public void SetItemActive(int itemNumber){
		if (!activeItem.Equals (itemsToBuy [itemNumber])) {
			activeItem.gameObject.SetActive (false);
			activeItem = itemsToBuy [itemNumber];
			activeItem.gameObject.SetActive (true);
			particles.Stop ();
			particles.Play ();

			BuyButton.SetActive (!itemsToBuy [itemNumber].owned);
			EquipButton.SetActive (itemsToBuy [itemNumber].owned);

			itemText.text = activeItem.name;
		}
	}

	//For updating the text field in the UI of the shop
	void UpdateCoinText(){
		if (coinsToSpend != null && gameManager != null) {
			coinsToSpend.text = "Coins: " + gameManager.data.coins.ToString ();
		}
	}

	//This method is probably gonna used by a button in the UI of the shop.
	public void BuyItem(){
		if (gameManager.data.coins >= activeItem.price && !activeItem.owned) {
			gameManager.data.coins -= activeItem.price;
			UpdateCoinText ();

			//Set Equip button to true after purchasing
			BuyButton.SetActive (false);
			EquipButton.SetActive (true);

			SetOwned (activeItem);
		}
	}

	public void EquipItem(){
		int type = activeItem.type;

		for (int i = 0; i < itemsToBuy.Length; i++) {
			if (itemsToBuy [i].type == type) {
				itemsToBuy [i].shopInterfaceButton.setOwned ();
				itemsToBuy [i].equipped = false;
			}
		}

		activeItem.shopInterfaceButton.setEquipped ();
		activeItem.equipped = true;
		activeItem.Equip ();
	}

	//Disable the price in the shop Canvas and enable the OWNED text.
	public void SetOwned(int itemNumber){
		BuyableItem buyableItem = itemsToBuy [itemNumber];
		SetOwned (buyableItem);
	}

	public void SetOwned(BuyableItem buyableItem){
		ShopInterfaceButton shopInterfaceButton = buyableItem.shopInterfaceButton;
		shopInterfaceButton.setOwned ();
		buyableItem.owned = true;
	}

	public void ToggleShop(){
		gameManager.ToggleShop ();
	}

	public void EquipActives(){
		for (int i = 0; i < itemsToBuy.Length; i++) {
			if (itemsToBuy [i].equipped) {
				itemsToBuy [i].Equip ();
			}
		}
	}
}
