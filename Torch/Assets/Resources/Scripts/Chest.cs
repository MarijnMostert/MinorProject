using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Chest : InteractableItem {

    [HideInInspector]
	public GameObject[] contents;

	public override void action(GameObject triggerObject){
		for(int i = 0; i < contents.Length; i++){
			flyOut (contents [i]);
			Destroy (gameObject);
		}
	}

	void flyOut(GameObject obj){
		float randomX = (1f - 2f * Random.value) * 2;
		float randomZ = (1f - 2f * Random.value) * 2;
		Vector3 spawnLocation = new Vector3(transform.position.x + randomX, .5f, transform.position.z + randomZ);
		Instantiate (obj, spawnLocation, transform.rotation);
	}

    public void addItem(GameObject item)
    {
        System.Array.Resize(ref contents,contents.Length+1);
        contents[contents.Length-1] = item;
    }
}