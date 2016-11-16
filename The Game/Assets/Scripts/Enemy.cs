using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IDamagable {

	public int health;
	public NavMesh navMesh;
	public float attackCooldown;
	public int attackDamage;
	public float refreshTime = 0.1f;
	public float attackRange = 1f;

	private GameObject target;
	private NavMeshAgent navMeshAgent;
	private float lastAttackTime;

	void Awake() {
		navMeshAgent = GetComponent<NavMeshAgent> ();
	}

	void Start () {
		target = GameObject.FindGameObjectWithTag ("Torch");
		lastAttackTime = Time.time;

		//Start the coroutine of travelling towards
		StartCoroutine (UpdatePath ());
	}

	void Update(){
		if (target != null) {
			doDamage ();
		}
	}


	//A Coroutine for chasing a target
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

	//If the player is close enough to the torch it will do damage
	private void doDamage(){
		float distance = distanceToTorch ();
		if(distance < attackRange && (Time.time - lastAttackTime) > attackCooldown){
			target.GetComponent<Torch> ().takeDamage (attackDamage);
			lastAttackTime = Time.time;
		}
	}

	//Get the distance between the enemy and the torch
	private float distanceToTorch(){
		Vector3 distV3 = transform.position - target.transform.position;
		float distFl = Mathf.Abs(distV3.magnitude);
		return distFl;
	}

	public void takeDamage(int damage){
		health -= damage;
		if (health <= 0)
			die ();
	}

	private void die(){
		Destroy (gameObject);
	}
}
