using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

	public float[][] weights;
	public float[] input;
	public GameObject target;
	public ArrayList bulletsNearby;
	public GameObject bulletNearby1;
	public GameObject bulletNearby2;

	// Use this for initialization
	void Start () {
		target = GameObject.Find ("Player");
		for (int i = 0; i < weights.Length; i++) {
			for (int j = 0; j < weights [i].Length; j++) {
				weights [i] [j] = 0;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		input [0] = target.transform.position.x - transform.position.x;
		input [1] = target.transform.position.y - transform.position.y;
		findClosestBullets ();
		input [2] = 
	}

	void findClosestBullets(){
		GameObject[] twoBullets = new GameObject[2];

		GameObject[] bulletsNearbyA = bulletsNearby.ToArray() as GameObject[];
		twoBullets [0] = bulletsNearbyA [0];
		twoBullets [1] = bulletsNearbyA [1];
		if(bulletsNearbyA.Length > 2){
			for (int i = 0; i < bulletsNearbyA.Length; i++){
				if(getDist(bulletsNearbyA[i]) < getDist(twoBullets[0])){
					twoBullets [0] = bulletsNearbyA [i];
				}
				else{
					if(getDist(bulletsNearbyA[i]) < getDist(twoBullets[1])){
						twoBullets [1] = bulletsNearbyA [i];
					}
				}
			}
		}
					

		//foreach (Object obj in bulletsNearby) {
		//	GameObject gobj = (GameObject)obj;
		//	if ((gobj.transform.position - transform.position).magnitude < ) {

		//	}
		//}
	}

	float getDist(GameObject other){
		return (other.transform.position - gameObject.transform.position).magnitude;
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Bullet")) {
			bulletsNearby.Add (other.gameObject);
		}
	}

	void OnTriggerStay(Collider other){
		if(other.gameObject.CompareTag("Bullet") && !bulletsNearby.Contains(other.gameObject)){
			bulletsNearby.Add (other.gameObject);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.CompareTag ("Bullet")) {
			bulletsNearby.Remove (other.gameObject);
		}
	}
}
