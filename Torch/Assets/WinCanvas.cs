using UnityEngine;
using System.Collections;

public class WinCanvas : MonoBehaviour {

	public void Proceed(){
		GameManager.Instance.TransitionDeathToMain ();
	}

	public void PlayUISound(){
		GameManager.Instance.playUISound (0);
	}
}
