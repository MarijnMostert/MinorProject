using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	public int scoreValue = 10;

	public virtual void Use(){
		GetComponentInParent<PlayerData> ().IncrementScorePickedUp (scoreValue);
		GameManager.Instance.updateScore (scoreValue);
	}
}
