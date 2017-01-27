using UnityEngine;
using System.Collections;

public class PlayOffline : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void playOffline()
    {
        GameManager.Instance.dungeonStartCanvas.buttons.Clear();
        GameManager.Instance.SetUpDungeonStartCanvas();
        HomeScreenProgress.Instance.UpdateProgress(GameManager.Instance.data.maxAchievedDungeonLevel);

        Time.timeScale = 1;
        transform.parent.gameObject.SetActive(false);
        GameManager.Instance.SetTextFieldEnabled(false);
        GameManager.Instance.homeScreenMovement.enabled = true;
    }
}
