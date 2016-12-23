using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : Object {

	private p2D center;
	private List<p2D> doorways;
	private int radius;
	private int[] directiondoors;
	private static int[,] mymaze;

	public static void setMaze(int[,] maze) {
		mymaze = maze;
	}

	public Room (p2D c, int radius) {
		this.center = c;
		this.radius = radius;
		this.doorways = new List<p2D> ();
		this.directiondoors = new int[] {0,0,0,0};
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

	public int[] getDirectiondoors () {
		return directiondoors;
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

	public void goRound() {
		if (mymaze [center.getX () + radius + 1, center.getY ()] == 1) {
			directiondoors [2] = 1;
		}
		if (mymaze [center.getX () - radius - 1, center.getY ()] == 1) {
			directiondoors [0] = 1;
		}
		if (mymaze [center.getX (), center.getY () + radius + 1] == 1) {
			directiondoors [1] = 1;
		}
		if (mymaze [center.getX (), center.getY () - radius - 1] == 1) {
			directiondoors [3] = 1;
		}				
	}

}