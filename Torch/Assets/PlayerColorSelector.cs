using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerColorSelector : MonoBehaviour {

	Color color;

	void Start () {
		color = GetComponent<Image> ().color;
	}
	
	public void SetPlayerColor(int playerNumber){
		GameManager.Instance.playerManagers [playerNumber - 1].SetPlayerColor (color);
	}

	public void PlayUISound(){
		GameManager.Instance.playUISound (0);
	}
}
