using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Chest : InteractableItem {

	public GameObject[] contents;

	[Server]
	public override void action(){
		print ("into action");
		for(int i = 0; i < contents.Length; i++){
			CmdServerAssignClient ();
			CmdFlyOut (contents [i]);
			CmdServerRemoveClient ();
		}
	}

	[Command]
	void CmdServerAssignClient()
	{
		GameObject chest = GameObject.Find("Chest");
		chest.GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);

	}

	[Command]
	void CmdServerRemoveClient()
	{
	GameObject chest = GameObject.Find("Chest");

	chest.GetComponent<NetworkIdentity>().RemoveClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);

	}

	[Command]
	void CmdFlyOut(GameObject obj){
		float randomX = (1f - 2f * Random.value) * 2;
		float randomZ = (1f - 2f * Random.value) * 2;
		Vector3 spawnLocation = new Vector3(transform.position.x + randomX, .5f, transform.position.z + randomZ);
		obj = Instantiate (obj, spawnLocation, transform.rotation) as GameObject;
		NetworkServer.Spawn (obj);
	}
}