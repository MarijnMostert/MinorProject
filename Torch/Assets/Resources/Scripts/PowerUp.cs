using UnityEngine;
using System.Collections;

public class PowerUp : AudioObject {

	public int scoreValue = 10;
	public AudioClip ClipOnUse;

	public virtual void Use(){
		if (ClipOnUse != null) {
			ObjectPooler.Instance.PlayAudioSource (ClipOnUse, mixerGroup, pitchMin, pitchMax, transform);
		}
		GetComponentInParent<PlayerData> ().IncrementScorePickedUp (scoreValue);
		GetComponentInParent<PlayerData> ().IncrementPowerUpsUsed (1);
		GameManager.Instance.updateScore (scoreValue);
	}
}
