using UnityEngine;
using System.Collections;

public class StartChoice : MonoBehaviour {
	public GameManager gameManager;
	private Data data;

	void Start () {
		gameManager = GameManager.Instance;
		data = gameManager.data;
	}

	public void ContinueGame(){
		GameManager.Instance.dungeonStartCanvas.buttons.Clear();
		GameManager.Instance.SetUpDungeonStartCanvas();
		this.gameObject.SetActive(false);
	}
	public void NewGame (string choice){
		data.ResetData ();
		switch (choice) {
		case "FromScratch":
			break;

		case "GoodStart":
			data.shopItemsOwned = new bool[]{
				true, false, true, false, false,
				false, false, false, false, false,
				true, false, false, false, false,
				false, false, true, false, false,
				false, false, true, false, false,

				false, false, false, false, false,
				false, false, false, false, false,
				false, false, false, false, false,
				false, false, false, false, false,
				false, false, false, false, false,};
			data.shopItemsEquipped = new bool[]{
				true, false, true, false, false,
				false, false, false, false, false,
				true, false, false, false, false,
				false, false, true, false, false,
				false, false, true, false, false,

				false, false, false, false, false,
				false, false, false, false, false,
				false, false, false, false, false,
				false, false, false, false, false,
				false, false, false, false, false,};

			data.coins = 1000;
			data.maxAchievedDungeonLevel = 12;
			break;

		case "EverythingUnlocked":

			data.shopItemsOwned = new bool[]{
				true, true, true, true, true,
				true, true, true, true, true,
				true, true, true, true, true,
				true, true, true, true, true,
				true, true, true, true, true,
			
				true, true, true, true, true,
				true, true, true, true, true,
				true, true, true, true, true,
				true, true, true, true, true,
				true, true, true, true, true};
			data.coins = 100000;
			data.maxAchievedDungeonLevel = 50;
			break;

		}/**/

		GameManager.Instance.SetUpDungeonStartCanvas();
		HomeScreenProgress.Instance.UpdateProgress(GameManager.Instance.data.maxAchievedDungeonLevel);

		gameManager.Start ();
		this.gameObject.SetActive(false);

	}
}