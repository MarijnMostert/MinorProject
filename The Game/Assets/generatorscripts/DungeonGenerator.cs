using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour {

	public int width;
	public int height;
	public int roomRadius;
	public int corridorLength;
	public Room ROOM;
	private int[,] dungeonMaze = new int[width,height];

	//private List<Space> spaces;
	public List<p2D> doorways;

	//Initialize area with no rooms nor corridors
	public DungeonGenerator(int width, int height, int roomRadius, int corridorLength) {
		this.width = width;
		this.height = height;
		this.roomRadius = roomRadius;
		this.corridorLength = corridorLength;

		/*
		for (int x = 0; x < width ; x++) {
			for (int y = 0; y < height ; y++) {
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					dungeonMaze [x, y] = 1;
				} else {
					dungeonMaze [x, y] = 0;
				}
			}
		}*/
		//spaces = new List<Space>();
	}



	//Build the first room
	public void Start() {
		dungeonMaze = new int[width, height];
		doorways = new List<p2D> ();

		for (int x = 0; x < width ; x++) {
			for (int y = 0; y < height ; y++) {
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					dungeonMaze [x, y] = 1;
				} else {
					dungeonMaze [x, y] = 0;
				}
			}
		}

		p2D center = new p2D(width/2, height/2);
		buildRoom (center);

		generate();
	}

	//Start generating the rest
	private void generate() {
		int i = 0;
		while(i < 4){
			KeyValuePair<p2D, int> checkFrom = pickDoorway ();

			string choice = chooseBuild ();
			switch (choice) {
			case "room":
				if (roomFree (checkFrom.Key, checkFrom.Value)) {
					buildRoomDirectional (checkFrom.Key, checkFrom.Value);
					i++;
				}
				break;
			case "corridor":
				if (corridorFree (checkFrom.Key, checkFrom.Value)) {
					buildCorridor (checkFrom.Key, checkFrom.Value);
					i++;
				}
				break;
			}
		}
	}

	//Choose to build from the available elements
	private static string chooseBuild(){
		string[] choices = new string[2]{ "room", "corridor" };
		int choose = Random.Range (0, 2);

		return choices [choose];
	}

	private int[] getSurrounding(p2D point) {
		int x = point.getX();
		int y = point.getY();
		return new int[4] {
			dungeonMaze [x + 1, y], //North
			dungeonMaze [x, y + 1], //East
			dungeonMaze [x - 1, y], //South
			dungeonMaze [x, y - 1]  //West
		};
	}

	//Find the direction that the chosen door is facing
	private int findDirection(p2D coord) {
		int[] surrounding = getSurrounding (coord);
		int direction = -1;

		//This is the case for the two opposing directions being different
		//So 0 and 2 are compared and 1 and 3
		//Then the direction is chosen of the two that goed to a value 0
		if (surrounding [0] != surrounding [2]) {
			if (surrounding [0] == 0) {
				direction = 0;
			} else if (surrounding [2] == 0) {
				direction = 2;
			}
		} else if (surrounding [1] != surrounding [3]) {
			if (surrounding [1] == 0) {
				direction = 1;
			} else if (surrounding [3] == 0) {
				direction = 3;
			}
		}

		return direction;
	}

	//Picks a doorway from the current list of doorways and finds its direction
	private KeyValuePair<p2D, int> pickDoorway() {
		//Clean up the list - only look at doors that are not interconnected yet
		List<p2D> chooseFrom = noDupes(doorways);

		//Pick a random number, smaller than the size of the array to choose from
		int choose = 0;//Random.Range (0, chooseFrom.Count);

		//Save the door at the chosen index
		p2D choice = chooseFrom [choose];

		//Find out its direction
		int direction = findDirection(choice);

		return new KeyValuePair<p2D, int> (choice, direction);
	}

	//Reduces the list to only the doors that are not yet connected.
	private static List<p2D> noDupes(List<p2D> list){
		List<p2D> unique = new List<p2D>();
		List<int> amounts = new List<int>();

		//Make a list of the unique elements in list, together with a list of how many times those elements appeared
		foreach (var element in list){
			//If not yet in it, add it and set amount to 1
			if (!unique.Contains (element)) {
				unique.Add (element);
				amounts.Add (1);
			} 
			//If already in it, up the amount by 1
			else {
				int index = unique.IndexOf (element);
				amounts[index] ++; 
			}
		}

		//Now mark the elements that appear more than once (overlapping doors)
		List<p2D> marked = new List<p2D>();
		for (int i = 0 ; i < amounts.Count ; i ++){
			if (amounts[i] > 1) {
				marked.Add (unique[i]);
			}
		}

		//Remove the marked elements from the list
		foreach (var element in marked) {
			unique.Remove(element);
		}

		//Return the now cleaned up list
		return unique;
	}

	//Checks available space from selected wall
	private bool lineFree(p2D coord, int direction, int searchLength) {
		int x = coord.getX ();
		int y = coord.getY ();
		switch (direction) {
		case 1: //East 
			for (int i = x; i < x + searchLength; i++) {
				if (dungeonMaze [i, y] == 1) {
					return false;
				}
			}
			break;
		case 3: //West
			for (int i = x; i > x - searchLength; i--) {
				if (dungeonMaze[i,y] == 1) {
					return false;
				}
			}
			break;
		case 0: //North
			for (int i = y; i < y + searchLength; i++) {
				if (dungeonMaze[x,i] == 1) {
					return false;
				}
			}
			break;
		case 2: //South
			for (int i = y; i> y - searchLength; i--) {
				if (dungeonMaze[x,i] == 1) {
					return false;
				}
			}
			break;
		}

		return true;
	}

	//Checks available space of a recangle, plus a row of space around it
	private bool rectangleFree(p2D coord, int direction, int rectangleWidth, int rectangleLength) {
		int searchWidth = rectangleWidth + 1;
		int x = coord.getX ();
		int y = coord.getY ();

		switch (direction) {
		case 0:
		case 2: //Then you're on a horizontal wall
			int minX = x - searchWidth;
			int maxX = x + searchWidth;

			for (int i = minX; i < maxX ; i++) {
				if (!lineFree (new p2D(i, y), direction, rectangleLength)) {
					return false;
				}
			}
			break;
		case 1:
		case 3: //Then you're on a vertical wall
			int minY = y - searchWidth;
			int maxY = y + searchWidth;

			for (int i = minY; i < maxY ; i++) {
				if (!lineFree (new p2D(x, i), direction, rectangleLength)) {
					return false;
				}
			}
			break;
		}

		return true;
	}

	//Sees if there's space for a room
	private bool roomFree (p2D coord, int direction) {
		return rectangleFree (coord, direction, roomRadius, roomRadius);
	}
	//Sees if there's space for a corridor
	private bool corridorFree (p2D coord, int direction) {
		return rectangleFree (coord, direction, 1, corridorLength);
	}

	//Move over the given coordinate so that the center build method can be used
	private void buildRoomDirectional(p2D start, int direction){
		start.translateDirecional (roomRadius, direction);
		buildRoom (start);
	}

	//Build a room with arguments as center of the room
	private void buildRoom(p2D center) {
		Room room = new Room (center, roomRadius);
		this.ROOM = room;

		int minX = center.getX() - roomRadius;
		int maxX = center.getX() + roomRadius + 1;
		int minY = center.getY() - roomRadius;
		int maxY = center.getY() + roomRadius + 1;

		Debug.Log (minX + " " + minY + " " + maxX + " " + maxY);


		for (int x = minX; x < maxX; x++) {
			for (int y = minY; y < maxY; y++) {
				dungeonMaze [x, y] = 1;
			}
		}

		Debug.Log ("Maze updated");
		Debug.Log (center.getX () + roomRadius + 1 + " " + center.getY());
		//spaces.Add(room);
		p2D first = new p2D(center.getX() + roomRadius + 1, center.getY());
		Debug.Log (first.getX());

		this.doorways.Add(first);
		this.doorways.Add(new p2D(center.getX() - roomRadius + 1, center.getY()));
		this.doorways.Add(new p2D(center.getX(), center.getY() + roomRadius + 1));
		this.doorways.Add(new p2D(center.getX(), center.getY() - roomRadius - 1));		
	}

	private void buildCorridor(p2D start, int direction) {
		p2D end = null;
		int x = start.getX();
		int y = start.getY();

		switch (direction) {
		case 1: //East 
			for (int i = x; i <= x + corridorLength; i++) {
				dungeonMaze [i, y] = 1;
				end.setP (i, y);
			}
			break;
		case 3: //West
			for (int i = x; i >= x - corridorLength; i--) {
				dungeonMaze[i, y] = 1;
				end.setP (i, y);
			}
			break;
		case 0: //North
			for (int i = y; i <= y + corridorLength; i++) {
				dungeonMaze [x, i] = 1;
				end.setP (x, i);
			}
			break;
		case 2: //South
			for (int i = y; i >= y - corridorLength; i--) {
				dungeonMaze[x,i] = 1;
				end.setP (x, i);
			}
			break;
		}

		//Corridor corridor = new Corridor (start, end, direction);
		//spaces.Add (corridor);
		doorways.Add (end); 
		doorways.Add (start);
	}
}
