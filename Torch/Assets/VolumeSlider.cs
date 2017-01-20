using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour {

	public void AdjustVolume(float newVolume){
		AudioListener.volume = newVolume;
	}
}
