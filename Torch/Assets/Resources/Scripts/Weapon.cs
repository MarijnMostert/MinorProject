using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class Weapon : AudioObject {
    public Sprite icon;
	public AudioClip fireClip;

	public virtual void Fire(){
	}
}
