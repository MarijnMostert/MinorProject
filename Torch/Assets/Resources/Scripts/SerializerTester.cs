using UnityEngine;
using System.Collections;

public class SerializerTester : MonoBehaviour {

	public int[] intArray;

	void Start(){
		intArray = new int[5];
		for (int i = 0; i < intArray.Length; i++) {
			intArray [i] = 5 + 3 * i;
		}
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Alpha6)){
			Serializer.Save<int[]>("serializerTester.txt", this.intArray);
			Debug.Log ("Save");
		}

		if (Input.GetKeyDown (KeyCode.Alpha7)) {
			intArray = null;
			Debug.Log ("Reset");
		}

		if (Input.GetKeyDown (KeyCode.Alpha8)) {
			intArray = Serializer.Load<int[]> ("serializerTester.txt");
			Debug.Log ("Load");
		}

	}
}
