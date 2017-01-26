using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressObject : MonoBehaviour {

	private HomeScreenProgress homeScreenProgress;
	[Header("- General:")]
	public float beginAt;
	public float endAt;


	[Header("- if ParticleSystem:")]
	public bool boolParticles;
	public float startAmountOfParticles;
	public float endAmountOfParticles;


	[Header("- if Light:")]
	public bool boolLight;
	public bool changeColor;
	public Gradient lightColor;
	public bool changeRange;
	public float startRange;
	public float endRange;

	[Header("- if Text:")]
	public bool boolText;
	public Gradient textColor;

	void Start () {
		homeScreenProgress = HomeScreenProgress.Instance;
		homeScreenProgress.progressObjects.Add (this);
	}

	public void UpdateOnProgress(float progress){
		if (boolParticles) {
			var em = GetComponent<ParticleSystem> ().emission;
			if (progress < beginAt) {
				em.rate = startAmountOfParticles;
			} else if (progress >= beginAt && progress < endAt) {
				em.rate = Mathf.Lerp (startAmountOfParticles, endAmountOfParticles, (progress - beginAt) / (endAt - beginAt));
			} else {
				em.rate = endAmountOfParticles;
			}
		} else if (boolLight) {
			Light li = GetComponent<Light> ();
			if (progress < beginAt) {
				if (changeColor)
					li.color = lightColor.Evaluate (0f);
				if (changeRange)
					li.range = startRange;
			} else if (progress >= beginAt && progress < endAt) {
				if (changeColor)
					li.color = lightColor.Evaluate ((progress - beginAt) / (endAt - beginAt));
				if (changeRange)
					li.range = Mathf.Lerp (startRange, endRange, (progress - beginAt) / (endAt - beginAt));
			} else {
				if (changeColor)
					li.color = lightColor.Evaluate (1f);
				if (changeRange)
					li.range = endRange;
			}
		} else if (boolText) {
			Text text = GetComponent<Text> ();
			if (progress < beginAt) {
				text.color = textColor.Evaluate (0f);
			} else if (progress >= beginAt && progress <= endAt) {
				text.color = textColor.Evaluate ((progress - beginAt) / (endAt - beginAt));
			} else {
				text.color = textColor.Evaluate (1f);
			}
		}
	}
}
