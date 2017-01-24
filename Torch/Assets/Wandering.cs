using UnityEngine;
using System.Collections;

public class Wandering : MonoBehaviour {
	public Transform center;
	public Vector3 centerPosition;
	public float maxDistance;
	public float maxWalkTime;
	public float maxWaitTime;
	public float maxTurnTime;
	public float minWalkTime;
	public float minWaitTime;
	public float minTurnTime;
	private float timeChosen;
	private float timeLeft;
	private bool walk;
	private bool turn;
	private Vector3 start;
	private Vector3 forward;
	private Vector3 target;
	// Use this for initialization
	void Start () {
		if (center == null) {
			center = transform;
		}
		centerPosition = center.position;
		start = centerPosition;
	}
	void rememberStartAndForward(){
		start = transform.position;
		forward = transform.forward;
	}
	Vector3 newTarget(){
		return new Vector3 (center.position.x+Random.Range(-maxDistance,maxDistance),center.position.y+Random.Range(-maxDistance,maxDistance),center.position.z+Random.Range(-maxDistance,maxDistance));	}
	// Update is called once per frame
	void Update () {
		timeLeft = timeLeft - Time.deltaTime;
		if (walk) {
			if (turn) {
				rotate ();
			} else {
				move ();
			}
		}
		if (timeLeft < 0.1) {
			if (turn) {
				turn = false;
				timeChosen = Random.Range(minWalkTime,maxWalkTime);
				timeLeft = timeChosen;
			} else {
				toggleWalk ();
				if (walk) {
					setupNewTurnAndWalk ();
				} else {
					setupNewWait ();
				}
			}
		}

	}
	void toggleWalk(){
		walk = !walk;
	}
	void move(){
		transform.position = target + (start - target) * timeLeft / timeChosen;
	}
	void rotate(){
		transform.forward = (target - start) + (forward - (target - start)) * timeLeft / timeChosen;
	}
	void setupNewWait(){
		timeChosen = Random.Range(minWaitTime,maxWaitTime);
		timeLeft = timeChosen;
	}
	void setupNewTurnAndWalk(){
		rememberStartAndForward ();
		target = newTarget ();
		turn = true;
		timeChosen = Random.Range(minTurnTime,maxTurnTime);
		timeLeft = timeChosen;
	}
}