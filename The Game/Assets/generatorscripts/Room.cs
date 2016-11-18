using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	public string naam = "kamor";
	private p2D center;
	private List<p2D> doorways;
	private int radius;

	public Room (int x, int y, int radius) {
		this.center = new p2D (x, y);
		this.radius = radius;
		doorways = new List<p2D> ();
		assignDoorways ();
	}

	public Room (p2D c, int radius) {
		this.center = c;
		this.radius = radius;
		this.doorways = new List<p2D> ();
		assignDoorways ();
	}

	private void assignDoorways(){
		this.doorways.Add(new p2D(center.getX() + radius + 1, center.getY()));
		this.doorways.Add(new p2D(center.getX() - radius + 1, center.getY()));
		this.doorways.Add(new p2D(center.getX(), center.getY() + radius + 1));
		this.doorways.Add(new p2D(center.getX(), center.getY() - radius - 1));							
	}

	public void removeDoorway(p2D door){
		this.doorways.Remove (door);
	}

	public List<p2D> getDoorways () {
		return this.doorways;
	}

}