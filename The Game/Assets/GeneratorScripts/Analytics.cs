using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Analytics : Object {

	private DungeonGenerator generator;
	private List<p2D> doorways;
	private int[,] maze;

	public int[,] getMaze (){
		return maze;
	}

	public Analytics (DungeonGenerator generator) {
		this.generator = generator;
		this.doorways = generator.getDoorways ();
		this.maze = generator.getMaze ();
	}

	public List<p2D> deadEnds () {
		List<p2D> deadEnds = new List<p2D> ();

		List<p2D> onlyUnconnected = DungeonGenerator.noDupes (doorways);
		foreach (var door in onlyUnconnected) {
			int direction = generator.findDirection (door);
			string previous = generator.findPrevious (new KeyValuePair<p2D,int> (door, direction));
			if (previous.Equals ("corridor")) {
				deadEnds.Add (door);
			}
		}
		return deadEnds;
	}

	public int[] roomDoorLocations (Room room) {
		int[] locations = new int[4] {0,0,0,0};
		for (int i = 0 ; i < room.getDoorways ().Count ; i ++) {
			p2D door = room.getDoorways ()[i];
			locations[i] = maze[door.getX (), door.getY()];
		}
		return locations;
	}

	public void cleanUpDeadEnds () {
		List<p2D> ends = deadEndsHard();
		while (ends.Count > 0) {
			foreach (var end in ends) {
				maze[end.getX (), end.getY ()] = 0;
			}
			ends = deadEndsHard ();
		}
	}

	public List<p2D> deadEndsHard () {
		List<p2D> ends = new List<p2D> ();
		for (int x = 2 ; x < generator.width - 2 ; x ++) {
			for (int y = 2 ; y < generator.height - 2 ; y ++) {
				int[] surrounding = generator.getSurrounding (new p2D(x,y));
				if ((surrounding[0] 
					+ surrounding[1] 
					+ surrounding[2]
					+ surrounding[3]) 
					== 1 && generator.getMaze()[x, y] == 1) 
				{
					ends.Add (new p2D(x, y));
				}
			}
		}
		return ends;
	}
}
