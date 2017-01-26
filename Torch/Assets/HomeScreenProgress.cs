using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HomeScreenProgress : MonoBehaviour {

	public static HomeScreenProgress Instance;
	public List<ProgressObject> progressObjects;
	public Gradient FogOverTime;

	[Range(0.0f, 1.0f)]
	public float progress;

	void Awake(){
		Instance = this;
	}

	void Start(){
		RenderSettings.fogColor = FogOverTime.Evaluate (0);
	}

	public void UpdateProgress(int maxAchievedLevel){
		progress = ((float)maxAchievedLevel) / 40f;
		StartCoroutine(UpdateHomeScreen ());
	}

	IEnumerator UpdateHomeScreen(){
		RenderSettings.fogColor = FogOverTime.Evaluate (progress);
		RenderSettings.fogDensity = Mathf.Lerp (.02f, .01f, progress);
		RenderSettings.skybox.SetFloat("_AtmosphereThickness", Mathf.Lerp (.51f, 2.74f, progress));

		for (int i = 0; i < progressObjects.Count; i++) {
			if (progressObjects [i] == null) {
				progressObjects.RemoveAt (i);
				i--;
			}
		}

		yield return null;

		foreach(ProgressObject PO in progressObjects){
			PO.UpdateOnProgress (progress);
		}
	}
}
