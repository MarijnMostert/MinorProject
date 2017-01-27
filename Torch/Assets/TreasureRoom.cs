using UnityEngine;
using System.Collections;

public class TreasureRoom : MonoBehaviour {

	public GameObject ChestPrefab;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < Random.Range(10, 25); i++) {
			GameObject obj = Instantiate (ChestPrefab, (transform.position + new Vector3 (Random.Range (-12f, 12f), 0f, Random.Range (-12f, 12f))), 
				Quaternion.Euler (new Vector3 (-90f, Random.Range (0, 360), 0f)), transform) as GameObject;
			int dungeonLevel = GameManager.Instance.dungeonLevel;
			if (dungeonLevel > 40)
				dungeonLevel = 40;
			obj.GetComponent<Chest> ().SetUp (GameManager.Instance.dungeonData.dungeonParameters [dungeonLevel], transform);
		}
	}
}
