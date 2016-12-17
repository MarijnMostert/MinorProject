using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamagePopUp : MonoBehaviour {

	public GameObject PopUp;
	public Camera cam;
	public float offset = 1f;

	void Update () {
		if (Input.GetMouseButtonDown (1)) {
			Debug.Log ("Click");
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			CreateDamagePopUp (Random.Range (10, 100), player, (Random.value < 0.05f));
		}
	}

	public void CreateDamagePopUp(int damage, GameObject target, bool crit){
		Color color;
		if (!crit) {
			color = Color.Lerp (Color.yellow, Color.red, damage / 100f);
		} else {
			color = Color.blue;
		}

		Vector3 location = cam.WorldToScreenPoint (target.transform.position + new Vector3(0f, offset, 0f));
		GameObject popup = Instantiate (PopUp, transform) as GameObject;
		popup.transform.position = location;
		popup.GetComponentInChildren<Text> ().text = damage.ToString();
		popup.GetComponentInChildren<Text> ().color = color;
	}
}
