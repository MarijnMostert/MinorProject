using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int health;
	public NavMesh navMesh;
	public float attackSpeed;
	public float refreshTime = 0.1f;

	private GameObject target;
	private NavMeshAgent navMeshAgent;

	void Awake() {
		navMeshAgent = GetComponent<NavMeshAgent> ();
	}

	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player");

		//Start the coroutine of travelling towards
		StartCoroutine (UpdatePath ());
	}

	private IEnumerator UpdatePath(){

		//First make sure there is a target
		while (target != null) {
			Vector3 targetPosition = new Vector3 (target.transform.position.x, 0, target.transform.position.z);

			//Set the target position for the Nav Mesh Agent
			navMeshAgent.SetDestination (targetPosition);

			//Make sure that the Nav Mesh Agent refreshes not every frame (to spare costs)
			yield return new WaitForSeconds (refreshTime);
		}
	}
}
