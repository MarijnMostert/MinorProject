using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Corridor : MonoBehaviour {

	private p2D start;
	private p2D end;
	private int direction;
	private List<p2D> doorways;

	public Corridor (int startX, int startY, int endX, int endY, int direction) {
		this.start = new p2D (startX, startY);
		this.end = new p2D (endX, endY);
		this.direction = direction;
		doorways = new List<p2D> ();
		assignDoorways ();
	}             

	public Corridor (p2D start, p2D end, int direction) {
		this.start = start;
		this.end = end;
		this.direction = direction;
		doorways = new List<p2D> ();
		assignDoorways ();
	}

	private void assignDoorways(){
		doorways.Add (start);
		doorways.Add (end);
	}
}
