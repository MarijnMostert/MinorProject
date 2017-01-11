using UnityEngine;
using System.Collections;

public class DungeonLevelButton : MonoBehaviour {

	[SerializeField] private int dungeonLevel;

	public void StartDungeon(){
		GameManager.Instance.ToggleDungeonStartCanvas ();
		GameManager.Instance.SetDungeonLevel (dungeonLevel);
		GameManager.Instance.StartGame ();
	}

	public void SetDungeonLevel(int dungeonLevel){
		this.dungeonLevel = dungeonLevel;
	}
}
