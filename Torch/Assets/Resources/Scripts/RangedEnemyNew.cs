using UnityEngine;
using System.Collections;

public class RangedEnemyNew : MonoBehaviour {

	private NavMeshAgent agent;
	public enum State {
		IDLE,
		CHASE
	}
		
	public State state = State.IDLE;
	private bool alive = true;

	public float speed = 1f;
	private float refreshTime = 0.2f;
	private GameManager gameManager;
	private WeaponController weaponController;
	public LayerMask layerMask;

	public bool UpdatingPath = false;
	public bool Attacking = false;
	public bool CanSeeTarget = false;

	void Awake(){
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
		weaponController = GetComponent<WeaponController> ();
	}

	void Start () {
		gameManager.enemyTarget = GameObject.Find ("HomeScreenPlayer");
		agent = GetComponent<NavMeshAgent> ();
		agent.SetDestination(gameManager.enemyTarget.transform.position);
		agent.speed = 0f;
		StartCoroutine (FSM ());
		StartCoroutine (UpdatePath());
		StartCoroutine (Attack());
		StartCoroutine (DetermineStoppingDistance ());
	}

	IEnumerator FSM(){
		while (alive) {
			switch (state) {
			case State.IDLE:
				Idle ();
				break;
			case State.CHASE:
				Chase ();
				break;
			}
			yield return null;
		}
	}

	void Idle(){
		UpdatingPath = false;
		Attacking = false;

	}

	void Chase(){
		UpdatingPath = true;
		Attacking = true;
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			state = State.CHASE;
			agent.speed = speed;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.CompareTag ("Player")) {
			state = State.IDLE;
			agent.speed = 0f;
		}
	}

	IEnumerator Attack(){
		while (gameManager.enemyTarget != null) {
			if (Attacking && CanSeeTarget) {
				weaponController.Fire ();
				Debug.Log ("Fire");
			}
			yield return new WaitForSeconds (1f);
		}
	}

	IEnumerator UpdatePath(){
		while (gameManager.enemyTarget != null) {
			if (UpdatingPath) {
				agent.SetDestination (gameManager.enemyTarget.transform.position);
			}
			yield return new WaitForSeconds (refreshTime);

		}
	}

	IEnumerator DetermineStoppingDistance(){
		while (gameManager.enemyTarget != null) {
			RaycastHit hit;
			Ray ray = new Ray (transform.position, transform.forward);
			if (Physics.Raycast (ray, out hit, layerMask)) {
				Debug.Log (hit.transform.gameObject);
				if (hit.transform.gameObject.CompareTag ("Player") || hit.transform.gameObject.CompareTag ("Torch")) {
					CanSeeTarget = true;
				} else {
					CanSeeTarget = false;
				}
			}
			yield return new WaitForSeconds (refreshTime);
		}
	}

}
