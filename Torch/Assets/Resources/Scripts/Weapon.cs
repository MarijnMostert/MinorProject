using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class Weapon : AudioObject {
    public Sprite icon;
	public AudioClip fireClip;
	[HideInInspector] public PlayerData playerData;
	public float damageMultiplier = 1;

	public virtual void Fire(){
		if (playerData != null) {
			playerData.IncrementShotsFired ();
		}
	}
}
