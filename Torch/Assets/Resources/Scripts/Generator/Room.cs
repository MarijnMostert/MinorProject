using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : Object {

	private p2D center;
	private List<p2D> doorways;
	private int radius;

	public Room (p2D c, int radius) {
		this.center = c;
		this.radius = radius;
		this.doorways = new List<p2D> ();
		assignDoorways ();
	}

	private void assignDoorways(){
		this.doorways.Add (new p2D(center.getX() + radius + 1, center.getY()));
		this.doorways.Add (new p2D(center.getX() - radius + 1, center.getY()));
		this.doorways.Add (new p2D(center.getX(), center.getY() + radius + 1));
		this.doorways.Add (new p2D(center.getX(), center.getY() - radius - 1));							
	}

	public void removeDoorway(p2D door){
		this.doorways.Remove (door);
	}

	public List<p2D> getDoorways () {
		return this.doorways;
	}

	public p2D getCenter () {
		return center;
	}

	public List<p2D> getAllCoords () {
		List<p2D> returnable = new List<p2D> ();

		for (int x = center.getX () - radius; x <= center.getX () + radius; x++) {
			for (int y = center.getY () - radius; y <= center.getY () + radius; y++) {
				returnable.Add (new p2D (x, y));
			}
		}
		return returnable;
	}

}