using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

	public GameManager gameManager;
	public BuyableItem[] itemsToBuy;
	[SerializeField] private ParticleSystem particles;
	private bool[] itemOwned;
	private BuyableItem activeItem;
	private int activeItemIndex;
	private Text moneyToSpend;
	private GameObject BuyButton;
	private GameObject EquipButton;

	void Awake(){
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
		moneyToSpend = transform.Find("Shop Canvas").Find ("MoneyToSpend").GetComponent<Text> ();
		particles = GetComponentInChildren<ParticleSystem> ();
		BuyButton = transform.Find ("Item Canvas").Find ("Buy Button").gameObject;
		EquipButton = transform.Find ("Item Canvas").Find ("Equip Button").gameObject;
	}

	void Start () {
		UpdateMoneyText ();

		//For safety, set every item to inactive
		foreach (BuyableItem BI in itemsToBuy) {
			BI.gameObject.SetActive (false);
		}

		//Set first item to active
		activeItem = itemsToBuy [0];
		activeItemIndex = 0;
		activeItem.gameObject.SetActive (true);
		itemOwned = new bool[itemsToBuy.Length];
		for (int i = 0; i < itemsToBuy.Length; i++) {
			itemOwned [i] = false;
		}

		//Set the buybutton active on default and the equipbutton inactive.
		BuyButton.SetActive (true);
		EquipButton.SetActive (false);
	}

	//Everytime the shop is set active, the available money text is being updated.
	void OnEnable(){
		UpdateMoneyText ();
	}

	//Method is going to be called by UI to show a preview of the item.
	public void SetItemActive(int itemNumber){
		if (!activeItem.Equals (itemsToBuy [itemNumber])) {
			activeItem.gameObject.SetActive (false);
			activeItem = itemsToBuy [itemNumber];
			activeItemIndex = itemNumber;
			activeItem.gameObject.SetActive (true);
			particles.Stop ();
			particles.Play ();

			if (itemOwned [itemNumber]) {
				BuyButton.SetActive (false);
				EquipButton.SetActive (true);
			} else {
				BuyButton.SetActive (true);
				EquipButton.SetActive (false);
			}

			transform.Find ("Item Canvas").Find ("Item Name").GetComponent<Text> ().text = activeItem.name;
		}
	}

	//For updating the text field in the UI of the shop
	void UpdateMoneyText(){
		moneyToSpend.text = "Money to spend: " + gameManager.money.ToString();
	}

	//This method is probably gonna used by a button in the UI of the shop.
	public void BuyItem(){
		if (gameManager.money >= activeItem.cost && !itemOwned[activeItemIndex]) {
			gameManager.money -= activeItem.cost;
			UpdateMoneyText ();

			itemOwned [activeItemIndex] = true;

			//Set Equip button to true after purchasing
			BuyButton.SetActive (false);
			EquipButton.SetActive (true);

			SetOwned (activeItemIndex);
		}
	}

	//Disable the price in the shop Canvas and enable the OWNED text.
	public void SetOwned(int itemNumber){
		Button[] buttons = GetComponentsInChildren<Button> ();
		foreach (Button button in buttons) {
			if(button.gameObject.name.EndsWith("Button" + itemNumber)){
				button.transform.Find ("Coin Image").gameObject.SetActive (false);
				button.transform.Find ("Cost").gameObject.SetActive (false);
				button.transform.Find ("Owned").gameObject.SetActive (true);
			}
		}
	}
}
