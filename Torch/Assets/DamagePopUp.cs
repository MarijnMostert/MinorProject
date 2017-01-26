using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamagePopUp : MonoBehaviour {

	static public GameObject PopUp;
	//static public Camera cam;
	static public float offset = 1f;

	/*
	void Update () {
		if (Input.GetMouseButtonDown (1)) {
			Debug.Log ("Click");
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			CreateDamagePopUp (Random.Range (10, 100), player, (Random.value < 0.05f));
		}
	}
	*/

	public static void CreateDamagePopUp(int damage, GameObject target, bool crit){
		Color color;
		if (!crit) {
			color = Color.Lerp (Color.yellow, Color.red, damage / 100f);
		} else {
			color = Color.cyan;
		}

		Vector3 location = target.transform.position + new Vector3 (0f, offset, 0f);

			//cam.WorldToScreenPoint (target.transform.position + new Vector3(0f, offset, 0f));
		GameObject popup = ObjectPooler.Instance.GetObject(0, true, location);
		//GameObject popup = Instantiate (PopUp) as GameObject;
		if (!crit) {
			popup.GetComponentInChildren<Text> ().text = damage.ToString ();
		} else if (damage < 70) {
			popup.GetComponentInChildren<Text> ().text = damage.ToString () + "!";
		} else if (damage < 150) {
			popup.GetComponentInChildren<Text> ().text = damage.ToString () + "!!";
		} else {
			popup.GetComponentInChildren<Text> ().text = damage.ToString () + "!!!";
		}
		popup.GetComponentInChildren<Text> ().color = color;
	}
}
