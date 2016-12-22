using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonGenerator : Object {

	//Hier een hele lange lijst met parameters
	//Dit zodat je ze makkelijk in de constructor kunt aanpassen
	public int width;								
	public int height;

	private int[,] dungeonMaze;					//Hierin sla je de maze op

	private bool done;
	public bool loopsAllowed;

	public int roomRadius;							//Radius van een kamer. 1 betekent een 3x3 kamertje
	public int maxCorridorLength;					//Er wordt een random getal tussen 1 en de max gekozen
	public int corridorLength;
	public int minAmountOfRooms;
	public int maxAmountOfRooms;
	public int chanceRoom;							//Er is een 1/chance kans dat hij een room kiest ipv een corridor

	private int loopsAllowedInt;
	private int mseconds;							//Miliseconden voordat hij terminate

	private List<p2D> doorways;
	private List<Room> rooms;
	private List<p2D> allRoomCoords;
	private List<p2D> roomCenters;


	//Deze functie is nodig voor in Unity, maar niet in Java.
	//Ik laat hem staan om later te kunnen heractiveren
	//Build the first room 
	/*
	public void Start() {
		DungeonGenerator dungeon = new DungeonGenerator(40,25,1,2,3000,6,13,6);
		Debug.Log ("done = " + dungeon.getRooms ().Count);
		Debug.Log (DungeonGenerator.noDupes(dungeon.doorways).ToString());
		Debug.Log (dungeonMaze);
	}
	*/


	//Initialize area with no rooms nor corridors
	public DungeonGenerator (int width, 
							 int height, 
							 int roomRadius, 
							 int maxCorridorLength,
							 int mseconds, 
							 int minAmountOfRooms, 
							 int maxAmountOfRooms, 
							 int chanceOfRoom) 
	{
		this.width = width;
		this.height = height;

		this.dungeonMaze = new int[width, height];
		this.doorways = new List<p2D> ();
		this.rooms = new List<Room> ();
		this.allRoomCoords = new List<p2D> ();
		this.roomCenters = new List<p2D> ();

		this.maxCorridorLength = maxCorridorLength;
		this.roomRadius = roomRadius;
		this.minAmountOfRooms = minAmountOfRooms;
		this.maxAmountOfRooms = maxAmountOfRooms;
		this.chanceRoom = chanceOfRoom;

		this.mseconds = mseconds;
		this.loopsAllowed = false;
		this.loopsAllowedInt = (loopsAllowed) ? 1:0;
		this.done = false;

		//Initializeer het dungeonMaze array met nullen (dicht) en enen (open) langs de rand
		//De enen zitten langs de rand omdat het algoritme niet kan bouwen waar al gebouwd is
		//Zo blijft hij bij de randen vandaan
		for (int x = 0; x < width ; x++) {
			for (int y = 0; y < height ; y++) {
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					dungeonMaze[x, y] = 1;
				} else {
					dungeonMaze [x, y] = 0;
				}
			}
		}

		//Begin met het bouwen van een room in het centrum van het vlak
		p2D center = new p2D (width/2, height/2);
		buildRoom (center);

		//Begin met genereren en geef terug in done of het gelukt is binnen de gegeven tijd.
		generate ();

		finish ();		//This function will tie up the ends of the maze
						//By adding rooms at the loose ends if possible

		Room.setMaze (dungeonMaze);
		foreach (Room room in rooms) {
			room.goRound ();
		}

		this.done = (rooms.Count >= minAmountOfRooms);
	}//constructor


	public int[,] getMaze () {
		return dungeonMaze;
	}


	public List<p2D> getDoorways () {
		return doorways;
	}


	public List<Room> getRooms () {
		return rooms;
	}

	public List<p2D> getRoomCenters () {
		foreach (Room room in rooms) {
			roomCenters.Add (room.getCenter ());
		}
		return roomCenters;
	}

	public List<p2D> getAllRoomCoords () {
		foreach (Room room in rooms) {
			foreach (p2D coord in room.getAllCoords ()) {
					allRoomCoords.Add (coord);
			}
		}
		return allRoomCoords;
	}

	public bool isDone () {
		return done;
	}


	//Start generating the rest of the maze
	private void generate () {
		int i = 0;
		float start = Time.realtimeSinceStartup * 1000;
		float endtime = start + mseconds; // 60 seconds * 1000 ms/sec
		int i_now = 0;
		int i_prev = -1;

		//While the desired amount of rooms is built or time has not yet run out
		while (i < minAmountOfRooms+1 && Time.realtimeSinceStartup * 1000 < endtime) {

			if (i_prev != i_now) {
				endtime = Time.realtimeSinceStartup * 1000 + mseconds;
			}

			i_prev = i_now;

			//Pick a length for your corridor
			this.corridorLength = Random.Range (0, maxCorridorLength)+1;

			//Choose one of the doors and the direction it points
			//From there we're going to check weither building is possible
			//The p2D Coordinate is the doorway coordinate
			//The integer Direction 0:north, 1:east, 2:south, 3:west
			KeyValuePair<p2D,int> checkFrom = pickDoorway ();
			//Debug.Log (checkFrom);


			//Check whether you can build from there (if the direction is legit)
			//if (checkFrom.Value == -1) { Debug.Log ("Continue"); continue; }

			//Find out what places the door will connect (from/to "corridor" or "room")
			string myfrom = findPrevious (checkFrom);
			string choice = chooseBuild (myfrom);


			switch (choice) {
			case "room":
				//If there is space for a room then build it and show the updated map
				if (roomFree (checkFrom.Key, checkFrom.Value)) {
					buildRoomDirectional (checkFrom.Key, checkFrom.Value);
					i++;
				}
				break;

			case "corridor":			
				//If you come from another corridor; maybe put in 90d turn
				int direction = checkFrom.Value;
				if (myfrom.Equals ("corridor")) { 
					//Add a number -1, 0, 1 to the current direction.
					//(%4 so the directions stay between 0 and 3)
					int number = Random.Range(-1,2);
					direction += (number + 4);
					direction %= 4;
					//Debug.Log ("Direction changed: " + direction);

				}

				//If there is space for a room then build it and show the updated map
				if (corridorFree (checkFrom.Key, direction, myfrom)) {
					buildCorridor (checkFrom.Key, direction);
				}
				break;
			}

			i_now = i;

		}		
	}//generate


	//The finish up function
	private void finish () {
		//restDoors are the leftover loose ends
		List<p2D> restDoors = noDupes (doorways);

		//Loop through all those
		foreach (var door in restDoors) {
			int direction = findDirection (door);

			//If the door comes from a corridor, and there is room for a room
			//Build the roo
			//Also if not over the room limitm
			string from = findPrevious (new KeyValuePair<p2D,int> (door, direction));
			if (maxAmountOfRooms >= rooms.Count 
				&& from.Equals ("corridor") 
				&& roomFree (door, direction)) 
			{
				buildRoomDirectional (door, direction);
			}
		}
	}//finish


	//Pick an possible build from the available elements
	private string chooseBuild (string from) {
		string[] choices = { "room", "corridor" };

		//Pick a number betwee one and changeRoom
		int number = Random.Range (0, chanceRoom);

		//If the number isn't zero; set it to 1 - the choice for a corridor
		//This makes sure that the chance for a room is 1/chanceRoom
		if (number > 0) { number = 1; }

		//It is not allowed to place two rooms next to each other.
		//So if you come from a room, you automatically choose a corridor
		if (from.Equals("room")) { return "corridor"; }

		//Or return the random choice
		return choices [number];
	}//chooseBuild


	//This function looks at the entourage of a coordinate, and outputs an array 
	//with the maze values at those coordinates in N-E-S-W order
	public int[] getSurrounding(p2D point) {
		int x = point.getX();
		int y = point.getY();

		//If you are within the boundaries of the maze, do the normal thing
		if (x > 0 && y > 0 && x < width-1 && y < height-1) {
			int[] array = {
				dungeonMaze [x, y + 1], //North
				dungeonMaze [x + 1, y], //East
				dungeonMaze [x, y - 1], //South
				dungeonMaze [x - 1, y]  //West
			};
			return array;
		}

		//As a catchnet - returning all 4 as built coordinates
		else {
			return new int[] {1,1,1,1};
		}
	}//getSurrounding


	//Find the direction that the chosen door is facing, by comparing its neighbours
	public int findDirection (p2D coord) {
		int[] surrounding = getSurrounding (coord);
		int direction = -1;

		//The direction is one of the two opposing directions that differ
		//So 0 and 2 are compared and 1 and 3 are
		//Then the direction is chosen of the two that has value 0
		/**
		 * 		000	
		 * 		121		The case for a room-door (2 is the door)
		 * `	111 
		 * 
		 * 		000
		 * 		020		The case for a corridor-door
		 * 		010
		 * 
		 * 		Other cases it will have trouble with recognizing - there we try a random direction
		 */

		if (surrounding [1] == surrounding [3] && surrounding[1] == 0) {
			if (surrounding [0] == 0 && surrounding[2] == 1) {
				direction = 0; //North
			} else if (surrounding [2] == 0 && surrounding[0] == 1) {
				direction = 2; //South
			}


		} else if (surrounding [0] == surrounding [2] && surrounding[0] == 0) {
			if (surrounding [1] == 0 && surrounding[3] == 1) {
				direction = 1; //East
			} else if (surrounding [3] == 0 && surrounding[1] == 1) {
				direction = 3; //West
			}	
		} 

		else if (surrounding[1] == surrounding[3] && surrounding[0] == surrounding[2] && surrounding[0] != surrounding[1]) {
			//System.err.println("Shortcorridor found");
			this.corridorLength = 0;
			return (surrounding[1] == 1) ? 1 : 0;
		}

		else {
			//Catch if anything goes wrong: then assign a random direction.
			direction = Random.Range (0, 4);
		}

		return direction;
	}//findDirection


	//Picks a doorway from the current list of doorways and finds its direction
	private KeyValuePair<p2D,int> pickDoorway() {

		//Clean up the list - only look at doors that are not interconnected yet
		List<p2D> chooseFrom = noDupes(doorways);
		//chooseFrom.Add (new p2D (0, 0));
		//doorways.Add (new p2D (0, 0));

		//Pick a random number, smaller than the size of the array to choose from
		int number = Random.Range (0, chooseFrom.Count);

		//Save the door at the chosen index
		//if (number < 1) { Debug.Log("empty nodupes list"); }
		p2D choice = chooseFrom[number];

		//Find out its direction
		int direction = findDirection (choice);

		return new KeyValuePair<p2D,int> (choice, direction);
	}//pickdoorways


	//This function determines what the door is currently connected to
	public string findPrevious (KeyValuePair<p2D,int> doorway) {
		p2D door = doorway.Key;				//Door coordinate
		int direction = doorway.Value;		//Door direction
		p2D check1 = new p2D(width/2,height/2);
		p2D check2 = new p2D(width/2,height/2);
		p2D temp = p2D.translateDirectional2 (door, 1, (direction+2)%4);

		check1 = p2D.translateDirectional2 (temp, 1, (direction+1)%4);
		check2 = p2D.translateDirectional2 (temp, 1, (direction+1)%4);

		/**
		 * Check 1 and check 2 are now (x being the door coordinate, > the direction)
		 * 
		 * 		100
		 * 		0X>
		 * 		200
		 */

		//Check weither at least one of them is occupied; then it's a room 
		//(or you at least won't be able to place a room next to it)
		if (dungeonMaze[check1.getX(), check1.getY()] == 1 
			|| dungeonMaze[check2.getX(), check2.getY()] == 1 ){
			return "room";
		}
		else return "corridor";
	}//findprevious


	//Reduces the list to only the doors that are not yet connected.
	//Doors that are already connected will be in the doors list twice
	//So filtering for unused doors is easy
	public static List<p2D> noDupes (List<p2D> list) {
		List<p2D> unique = new List<p2D> ();				//Contains unique elements
		List<int> amounts = new List<int> ();				//How often the unique elements 
		// are in the total list

		//Make a list of the unique elements in list, together with a list of how many times those elements appeared
		foreach (var element in list) {

			int index = p2D.myIndexOf (unique, element);
				
			//If not yet in it, add it and set amount to 1
			if (index == -1) {
				unique.Add (element);
				amounts.Add (1);
			} 
			//If already in it, up the amount by 1
			else {
				amounts [index] += 1;
			}
		}

		//Now collect the elements that appear more than once (the overlapping doors)
		//Create a list of those
		List<p2D> marked = new List<p2D> ();
		for (int i = 0 ; i < amounts.Count ; i ++) {
			//Which is thus when amount is more than one
			if (amounts[i] > 1) {
				marked.Add (unique[i]);
			}
		}

		p2D.myRemove (unique, marked);

		//Remove the marked elements from the list that will be returned
//		foreach (var element in marked) {
//			unique.Remove (element);
//		}

		//Return the now cleaned up list
		return unique;
	}//noDupes


	//Checks available space from selected point
	private bool lineFree(p2D coord, int direction, int lineLength) {
		int x = coord.getX ();
		int y = coord.getY ();
		int searchLength = lineLength;

		switch (direction) {
		case 1: //East 
			//If the coordinate is not within the available space; stop the procedure
			//This happens for all four cases
			if (x + searchLength >= width - 2) { return false; }

			for (int i = x; i <= x + searchLength; i++) {
				if (dungeonMaze [i, y] == 1) {
					return false;
				}
			}
			break;
		case 3: //West
			if (x - searchLength <= 1) { return false; }

			for (int i = x; i >= x - searchLength; i--) {
				if (dungeonMaze[i, y] == 1) {
					return false;
				}
			}
			break;
		case 0: //North
			if (y + searchLength >= height - 2) { return false; }

			for (int i = y; i <= y + searchLength; i++) {
				if (dungeonMaze[x, i] == 1) {
					return false;
				}
			}
			break;
		case 2: //South
			if (y - searchLength <= 1) { return false; }

			for (int i = y; i >= y - searchLength; i--) {
				if (dungeonMaze[x, i] == 1) {
					return false;
				}
			}
			break;
		default:
			//Debug.Log("Linefree didn't receive a correct direction");
			break;
		}

		return true;
	}//lineFree


	//Checks available space of a recangle, plus a row of space around it
	private bool rectangleFree(p2D coord, 
							   int direction,
							   int rectangleWidthRadius, 
							   int rectangleLength) 
	{
		int searchWidth = rectangleWidthRadius + 1;
		int x = coord.getX ();
		int y = coord.getY ();

		switch (direction) {
		case 0:
		case 2: //Cases whem you're on a horizontal wall
			int minX = x - searchWidth;
			int maxX = x + searchWidth;

			//Make sure you're looking within the available space
			if(minX <= 0 || maxX >= width - 1) { return false; }

			//Check all lines starting at the wall
			for (int i = minX; i <= maxX ; i++) {
				if (!lineFree (new p2D(i, y), direction, rectangleLength)) {
					return false;
				}
			}

			break;
		case 1:
		case 3: //Cases when you're on a vertical wall
			int minY = y - searchWidth;
			int maxY = y + searchWidth;

			//Make sure you're looking within the available space
			if(minY <= 0 || maxY >= height - 1) { return false; }

			//Check all lines starting at the wall
			for (int i = minY; i <= maxY ; i++) {
				if (!lineFree (new p2D(x, i), direction, rectangleLength)) {
					return false;
				}
			}
			break;
		default:
			//Debug.Log("Incorrect direction - not checked;");
			return false;
		}
		return true;
	}//rectangleFree


	//Sees if there's space for a room
	private bool roomFree (p2D coord, int direction) {
		//Needs to translate so that it doesn't start at the door itself, but one step further
		//Otherwise it will say false anyway
		p2D translated = p2D.translateDirectional2 (coord, 1, direction);
		return rectangleFree (translated, direction, roomRadius, (roomRadius*2)+1);
	}
	//Sees if there's space for a corridor
	private bool corridorFree (p2D coord, int direction, string from) {
		p2D translated = coord;
		//Needs to translate so that it doesn't start at the door itself, but one step further
		//Otherwise it will say false anyway
		if(true|| from.Equals ("room")) { translated = p2D.translateDirectional2 (coord, 1, direction); }
		return rectangleFree (translated, direction, 1, corridorLength-loopsAllowedInt);
	}

	//Move over the given coordinate so that the center build method can be used
	private void buildRoomDirectional(p2D start, int direction) {
		//The buildRoom function uses the room center, so we search for that place
		//using the door coordinate & direction
		p2D translated = p2D.translateDirectional2 (start, roomRadius + 1, direction);
		buildRoom (translated);
	}


	//Build a room using only the center of the room
	private void buildRoom(p2D center) {
		Room room = new Room(center, roomRadius);

		int minX = center.getX() - roomRadius;
		int maxX = center.getX() + roomRadius + 1;
		int minY = center.getY() - roomRadius;
		int maxY = center.getY() + roomRadius + 1;

		for (int x = minX; x < maxX; x++) {
			for (int y = minY; y < maxY; y++) {
				dungeonMaze [x,y] = 1;
			}
		}

		this.rooms.Add(room); //I keep a list of all the rooms for game purposes

		//Add the doorways of this room to the list
		//The doors are on all 4 walls in the center
		this.doorways.Add(new p2D(center.getX() + roomRadius + 1, center.getY()));
		this.doorways.Add(new p2D(center.getX() - roomRadius - 1, center.getY()));
		this.doorways.Add(new p2D(center.getX(), center.getY() + roomRadius + 1));
		this.doorways.Add(new p2D(center.getX(), center.getY() - roomRadius - 1));		
	}//buildRoom


	//Builds a corridor
	private void buildCorridor(p2D start, int direction) {
		//The start and end coordinates are saved
		p2D end = new p2D(0,0);
		int x = start.getX();
		int y = start.getY();

		switch (direction) {
		case 1: //East 
			for (int i = x; i <= x + corridorLength; i++) {
				dungeonMaze[i, y] = 1;
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
				dungeonMaze[x, i] = 1;
				end.setP (x, i);
			}
			break;
		case 2: //South
			for (int i = y; i >= y - corridorLength; i--) {
				dungeonMaze[x, i] = 1;
				end.setP (x, i);
			}
			break;
		default:
			//System.err.println("BuildCorridor end 0,0 added");
			break;
		}

		//The start and end are added to the list or doorwayss
		doorways.Add (end); 
		doorways.Add (start);
	}//buildCorridor
}