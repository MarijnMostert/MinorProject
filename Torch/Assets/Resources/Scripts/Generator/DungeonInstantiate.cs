using UnityEngine;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class DungeonInstantiate : Object {
    GameObject floor, side,
                            roof, block, trap_straight, trap_crossing, 
                            trap_box, portal, end_portal, player, 
                            game_manager, spawner, torch, 
                            cam, pointer, chest, 
                            roofGroup, wallTorch;
    GameObject[] starters_pack, chest_pack;
    //GameObject[,] dungeon;
    int[] mazeSize;
	bool[,] maze, import_maze, trapped;
    float chance_trap_straight, 
          chance_trap_crossing,
          chance_side_alt1, 
          chance_side_alt2, 
          step,
          chance_chest_corridors, 
          chance_chest_deadEnd,  
          chance_particles,
          chance_spidernest,
          chance_wizardnest,
          chance_wallspikes,
          chance_spikes,
          chance_shuriken,
          chance_wallrush;
    bool start_defined;
    int[] count = new int[2] {0,2};
	private GameObject Dungeon;

	List<GameObject> puzzleRooms;
	List<float> puzzleRoomSpawnChances;

	List<GameObject> traps;
	List<float> trapSpawnChances;

	List<p2D> puzzleCoords;
	List<p2D> puzzleCenters;
	List<Room> puzzleRoomsDG;

	GameObject WallsParent;
	GameObject FloorsParent;
	GameObject RoofsParent;
    GameObject Traps;
	GameObject PickupsParent;
	GameObject ChestsParent;
	GameObject WallTorchParent;
	GameObject ParticlesParent;
	GameObject BeginningRoom;
	GameObject EndingRoom;

	Vector3 startpoint;
	Vector3 endpoint;

	GameObject RoofPrefab;
	GameObject WallPrefab;
	GameObject DeadEndPrefab;
	GameObject CornerInnerPrefab;
	GameObject FloorPrefab;
	GameObject PuzzleMist;
	GameObject WoodenDoors;
	GameObject PuzzleDoors;

    GameObject spidernest;
    GameObject wizardnest;
    GameObject wallspikes;
    GameObject spikes;
    GameObject shuriken;
    GameObject wallrush;

	GameObject[] dungeonParticles;
	DungeonData.DungeonParameters dungeonParameters;

	public Vector3 startPos;

	StringBuilder sbPuzzles;
	StringBuilder sbTraps;

    // Use this for initialization
	public DungeonInstantiate(DungeonData.DungeonParameters dungeonParameters, GameObject floor, GameObject side, 
                            GameObject roof, GameObject block, GameObject trap_straight, GameObject trap_crossing, 
                            GameObject trap_box, GameObject portal, GameObject end_portal, GameObject player, 
                            GameObject game_manager, GameObject spawner, GameObject torch, GameObject cam, GameObject pointer, 
		GameObject chest, GameObject coin, GameObject fireball, GameObject iceball, GameObject health, int[] mazeSize, GameObject laser, GameObject shieldPickUp,
		GameObject stickyPickUp, GameObject roofGroup, GameObject wallPickUp, GameObject wallTorch, GameObject piercingWeapon,
		GameObject bombPickUp, GameObject spidernest, GameObject wizardnest, GameObject wallspikes, GameObject spikes, GameObject shuriken, GameObject wallrush, 
        GameObject stardustParticles, GameObject moondustParticles, GameObject decoyPickUp)

    {
		this.dungeonParameters = dungeonParameters;
        this.floor = floor;
        this.side = side;
        this.roof = roof;
        this.block = block;
        this.trap_straight = trap_straight;
        this.trap_crossing = trap_crossing;
        this.trap_box = trap_box;
        this.portal = portal;
        this.end_portal = end_portal;
        this.cam = cam;
        this.pointer = pointer;
        this.chest = chest;
        this.player = player;
        this.mazeSize = new int[2] { mazeSize[0] - 2, mazeSize[1] - 2 };
        this.spawner = spawner;
        this.game_manager = game_manager;
		this.puzzleCoords = new List<p2D> ();
		this.puzzleCenters = new List<p2D> ();
		this.puzzleRoomsDG = new List<Room> ();

        this.wizardnest = wizardnest;
        this.spidernest = spidernest;
        this.wallspikes = wallspikes;
        this.spikes = spikes;
        this.shuriken = shuriken;
        this.wallrush = wallrush;

		this.dungeonParticles = new GameObject[]{ stardustParticles, moondustParticles };

		WallsParent = new GameObject("Walls");
		FloorsParent = new GameObject("Floors");
		RoofsParent = new GameObject("Roofs");
        Traps = new GameObject("Traps");
		PickupsParent = new GameObject ("PickupsParent");
		ChestsParent = new GameObject ("ChestsParent");
		WallTorchParent = new GameObject ("WallTorchParent");
		ParticlesParent = new GameObject ("ParticlesParent");

		RoofPrefab = Resources.Load ("Prefabs/Blocks/RoofPrefab", typeof(GameObject)) as GameObject;
		WallPrefab = Resources.Load ("Prefabs/Blocks/WallPrefab", typeof(GameObject)) as GameObject;
		DeadEndPrefab = Resources.Load ("Prefabs/Blocks/DeadEndPrefab", typeof(GameObject)) as GameObject;
		CornerInnerPrefab = Resources.Load ("Prefabs/Blocks/CornerPrefab Inner", typeof(GameObject)) as GameObject;
		FloorPrefab = Resources.Load ("Prefabs/Blocks/FloorPrefab", typeof(GameObject)) as GameObject;
		PuzzleMist = Resources.Load("Prefabs/PuzzlesScenes/PuzzleMist", typeof(GameObject)) as GameObject;
		WoodenDoors =  Resources.Load("Prefabs/PuzzlesScenes/WoodenDoor", typeof(GameObject)) as GameObject;
		PuzzleDoors = Resources.Load("Prefabs/PuzzlesScenes/PuzzleDoors", typeof(GameObject)) as GameObject;

		BeginningRoom = Resources.Load ("Prefabs/PuzzlesScenes/BeginningRoom", typeof(GameObject)) as GameObject;
		EndingRoom = Resources.Load ("Prefabs/PuzzlesScenes/EndingRoom", typeof(GameObject)) as GameObject;


		this.starters_pack = new GameObject[] {torch};
		this.roofGroup = roofGroup;
		this.wallTorch = wallTorch;

		this.puzzleRooms = new List<GameObject> ();
		this.puzzleRoomSpawnChances = new List<float> ();
		if (dungeonParameters.puzzleRooms.Blockpuzzleroom.enabled) {
			puzzleRooms.Add (dungeonParameters.puzzleRooms.Blockpuzzleroom.puzzleRoom);
			puzzleRoomSpawnChances.Add (dungeonParameters.puzzleRooms.Blockpuzzleroom.spawnChance);
		}
		if (dungeonParameters.puzzleRooms.Fliproom.enabled) {
			puzzleRooms.Add (dungeonParameters.puzzleRooms.Fliproom.puzzleRoom);
			puzzleRoomSpawnChances.Add (dungeonParameters.puzzleRooms.Fliproom.spawnChance);
		}
		if(dungeonParameters.puzzleRooms.Bossroom.enabled){
			puzzleRooms.Add (dungeonParameters.puzzleRooms.Bossroom.puzzleRoom);
			puzzleRoomSpawnChances.Add (dungeonParameters.puzzleRooms.Bossroom.spawnChance);
		}
		if (dungeonParameters.puzzleRooms.Laserroom.enabled) {
			puzzleRooms.Add (dungeonParameters.puzzleRooms.Laserroom.puzzleRoom);
			puzzleRoomSpawnChances.Add (dungeonParameters.puzzleRooms.Laserroom.spawnChance);
		}
		if (dungeonParameters.puzzleRooms.Fallblockpuzzle.enabled) {
			puzzleRooms.Add (dungeonParameters.puzzleRooms.Fallblockpuzzle.puzzleRoom);
			puzzleRoomSpawnChances.Add (dungeonParameters.puzzleRooms.Fallblockpuzzle.spawnChance);
		}
		if (dungeonParameters.puzzleRooms.Movingplatformroom.enabled) {
			puzzleRooms.Add (dungeonParameters.puzzleRooms.Movingplatformroom.puzzleRoom);
			puzzleRoomSpawnChances.Add (dungeonParameters.puzzleRooms.Movingplatformroom.spawnChance);
		}
		if (dungeonParameters.puzzleRooms.Treasureroom.enabled) {
			puzzleRooms.Add (dungeonParameters.puzzleRooms.Treasureroom.puzzleRoom);
			puzzleRoomSpawnChances.Add (dungeonParameters.puzzleRooms.Treasureroom.spawnChance);
		}

		sbPuzzles = new StringBuilder ();
		sbPuzzles.Append ("Available puzzles in this dungeon: ");
		for(int i = 0; i < puzzleRooms.Count; i++) {
			sbPuzzles.Append ("\n" + puzzleRooms[i].name + ", chance: " + puzzleRoomSpawnChances[i].ToString());
		}

		NormalizePuzzleRoomChances ();

		this.traps = new List<GameObject> ();
		this.trapSpawnChances = new List<float> ();
		if (dungeonParameters.Traps.shuriken.enabled) {
			traps.Add (shuriken);
			trapSpawnChances.Add (dungeonParameters.Traps.shuriken.spawnChance);
		}
		if (dungeonParameters.Traps.spikes.enabled) {
			traps.Add (spikes);
			trapSpawnChances.Add (dungeonParameters.Traps.spikes.spawnChance);
		}
		if (dungeonParameters.Traps.wallrush.enabled) {
			traps.Add (wallrush);
			trapSpawnChances.Add (dungeonParameters.Traps.wallrush.spawnChance);
		}
		if (dungeonParameters.Traps.wallspikes.enabled) {
			traps.Add (wallspikes);
			trapSpawnChances.Add (dungeonParameters.Traps.wallspikes.spawnChance);
		}
		if (dungeonParameters.Traps.spidernest.enabled) {
			traps.Add (spidernest);
			trapSpawnChances.Add (dungeonParameters.Traps.spidernest.spawnChance);
		}
		if (dungeonParameters.Traps.wizardnest.enabled) {
			traps.Add (wizardnest);
			trapSpawnChances.Add (dungeonParameters.Traps.wizardnest.spawnChance);
		}

		sbTraps = new StringBuilder ();
		/*sbTraps.Append ("Available traps according to dungeonparameters:\n");
		if(dungeonParameters.Traps

*/


		sbTraps.Append ("Available traps in this dungeon: ");
		for(int i = 0; i < traps.Count; i++) {
			sbTraps.Append ("\n" + traps[i].name + ", chance: " + trapSpawnChances[i].ToString());
		}

		NormalizeTrapChances ();

    }

	public void setPuzzleCoords (List<p2D> coordList) {
		puzzleCoords = coordList;
	}

	public void setPuzzleCenters (List<p2D> coordList) {
		puzzleCenters = coordList;
	}

	public void setPuzzleRoomsDG (List<Room> list) {
		puzzleRoomsDG = list;
	}

    public void createMaze(){
		/*
        chance_trap_straight = 1f;
        chance_trap_crossing = 1f;
        chance_side_alt1 = 0.2f;
        chance_side_alt2 = 0.2f + chance_side_alt1;
        start_defined = false;
        */
        step = 6f;
		chance_chest_corridors = dungeonParameters.chanceChestCorridor;
		chance_chest_deadEnd = dungeonParameters.chanceChestDeadEnd;
		chance_particles = dungeonParameters.chanceParticles;

		/*
        //Compile Generated chances
        float norm = dungeonParameters.Traps.spidernest.spawnChance
            + dungeonParameters.Traps.spikes.spawnChance
            + dungeonParameters.Traps.wallspikes.spawnChance
            + dungeonParameters.Traps.wizardnest.spawnChance
            + dungeonParameters.Traps.wallrush.spawnChance
            + dungeonParameters.Traps.shuriken.spawnChance;
        if (norm != 0)
        {
            float temp = 0;
            chance_spidernest = (dungeonParameters.Traps.spidernest.spawnChance / norm); temp = chance_spidernest;
            chance_spikes = (dungeonParameters.Traps.spikes.spawnChance / norm) + temp; temp = chance_spikes;
            chance_wallspikes = (dungeonParameters.Traps.wallspikes.spawnChance / norm) + temp; temp = chance_wallspikes;
            chance_wizardnest = (dungeonParameters.Traps.wizardnest.spawnChance / norm) + temp; temp = chance_wizardnest;
            chance_wallrush = (dungeonParameters.Traps.wallrush.spawnChance / norm) + temp; temp = chance_wallrush;
            chance_shuriken = (dungeonParameters.Traps.shuriken.spawnChance / norm) + temp; temp = chance_shuriken;
        }
        else {
            chance_spidernest = 0;
            chance_spikes = 0;
            chance_wallspikes = 0;
            chance_wizardnest = 0;
            chance_wallrush = 0;
            chance_shuriken = 0;
        }
		*/

        //Instantiate empty Dungeon GameObject
        Dungeon = new GameObject("Dungeon");
		GameManager.Instance.levelTransform = Dungeon.transform;
		WallsParent.transform.SetParent(Dungeon.transform);
		FloorsParent.transform.SetParent(Dungeon.transform);
		RoofsParent.transform.SetParent(Dungeon.transform);
		Traps.transform.SetParent (Dungeon.transform);
		PickupsParent.transform.SetParent (Dungeon.transform);
		ChestsParent.transform.SetParent (Dungeon.transform);
		WallTorchParent.transform.SetParent (Dungeon.transform);
		ParticlesParent.transform.SetParent (Dungeon.transform);

        //import starters pack
		//InstantiateStarterPack(starters_pack, new Vector3(0, 0, 0),Quaternion.identity);
        //Instantiate(scene_manager, new Vector3(0, 0, 0), Quaternion.identity);

		spawner = Instantiate(spawner, new Vector3(0, 0, 0), Quaternion.identity, Dungeon.transform) as GameObject;
		Spawner spawnerScript = spawner.GetComponent<Spawner> ();
		spawnerScript.mapMinX = 0;
        spawnerScript.mapMinZ = 0;
        spawnerScript.mapMaxX = mazeSize[0]*6;
        spawnerScript.mapMaxZ = mazeSize[1]*6;
		spawnerScript.Setup (dungeonParameters);

		GameManager.Instance.spawner = spawner.GetComponent<Spawner> ();

        mazeSize = new int[] { mazeSize[0], mazeSize[1]};
        //dungeon = new GameObject[mazeSize[0], mazeSize[1]];
        maze = new bool[mazeSize[0], mazeSize[1]];
        maze = import_maze;

		populteMaze2 ();
		populatePuzzles ();

		//Debug.Log ("Localscale");
		WallsParent.transform.localScale = new Vector3 (6, 6, 6);
		FloorsParent.transform.localScale = new Vector3 (6, 1, 6);
		RoofsParent.transform.localScale = new Vector3 (6, 3, 6);

		Dungeon.transform.position = new Vector3 (0, 0, 0);

        createStartEndPoint();
        spawner.GetComponent<Spawner>().importMaze(maze, mazeSize);
    }


	void populteMaze2() {
		sbTraps.Append ("\n\nTraps in this dungeon:");

		for (int x = 0; x < mazeSize[0]; x++) {
			for (int y = 0; y < mazeSize[1]; y++) {
				bool puzzle = false;
				if (p2D.myContains (puzzleCoords, new p2D (x+1, y+1))) {
					puzzle = true; } // sla over
				if (maze[x,y]) {
					buildOpen (x,y,puzzle);
					if ((!puzzle) && Random.value < dungeonParameters.Traps.chanceForTrap && trapSpawnChances.Count != 0)
                    {
						StringBuilder sb = new StringBuilder ();
						sb.Append ("current coordinates: x:" + x + ",y:" + y + "    ");
						int[] surroundings = getSurrounding8 (x, y);
						int sum = 0;
						for (int i = 0; i < surroundings.Length; i++) {
							sum += surroundings [i];
							sb.Append (surroundings [i] + "  ");
						}
						sb.Append ("\nTotal number of surrounding floors: " + sum);
						if (sum >= 4) {
							sb.Append ("\nSo there will be no trap here");
						} else {
							sb.Append ("\nSo a trap will be placed here");
							trap(x, y);
						}
						//Debug.Log (sb.ToString ());
                    }
                }
                else {
					buildRoof (x, y);
				} 
			}
		}
		sbTraps.Append ("\n\n");
		Debug.Log (sbTraps.ToString ());
	}

	void buildRoof (int x, int y){
		GameObject myplane = GameObject.Instantiate (RoofPrefab, RoofsParent.transform) as GameObject;
		myplane.transform.position = new Vector3 (x+0.5f, 1f, y+0.5f);

		//Bouw navmeshblokkade
	}

	void buildOpen(int x, int y, bool puzzle) {
		if (!puzzle) {
			GameObject myplane = GameObject.Instantiate (FloorPrefab, FloorsParent.transform) as GameObject;
			myplane.transform.position = new Vector3 (x + 0.5f, 0, y + 0.5f);
			spawnChest (x, y);
			spawnWallTorch (x, y);
			spawnParticles (x, y);
		}


		//0 is floor, 1 is roof
		int[] arrayS = getSurrounding2 (x, y);
		int amountOfWalls = 0;
		for (int i = 0; i < arrayS.Length; i++) {
			if (arrayS [i] == 0)
				amountOfWalls++;
		}

		if (amountOfWalls == 1) {
			//Build one wall
			int rotation = 180;
			int direction = 0;
			for (int i = 0; i < arrayS.Length; i++) {
				if (arrayS [i] == 0) {
					BuildWall (rotation, direction % 4, x, y);
				}
				rotation -= 90;
				direction++;
			}


		}else if (amountOfWalls == 2) {
			if ((arrayS [0] == 1 && arrayS [1] == 0 && arrayS [2] == 1 && arrayS [3] == 0)
			    || (arrayS [0] == 0 && arrayS [1] == 1 && arrayS [2] == 0 && arrayS [3] == 1)) {
				//Build 2 walls
				int rotation = 180;
				int direction = 0;
				for (int i = 0; i < arrayS.Length; i++) {
					if (arrayS [i] == 0) {
						BuildWall (rotation, direction % 4, x, y);
					}
					rotation -= 90;
					direction++;
				}
			} else {
				int rotation = 0;
				int direction = 0;
				for (int i = 0; i < arrayS.Length; i++) {
					if (arrayS [i] == 0 && arrayS [(i + 1) % 4] == 0) {
						BuildCorner (rotation, direction % 4, x, y);
					}
					rotation -= 90;
					direction++;
				}
			}
		} else if (amountOfWalls == 3) {
			//Build Dead end
			int rotation = 0;
			int direction = 0;
			for (int i = 0; i < arrayS.Length; i++) {
				if (arrayS [i] == 1) {
					BuildDeadEnd (rotation, direction % 4, x, y);
				}
				rotation -= 90;
				direction++;
			}
		}



	}

	void BuildWall(int rotation, int direction, int x, int y){
		GameObject myplane = GameObject.Instantiate (WallPrefab, WallsParent.transform) as GameObject;
		Vector3 transformvector;
		Vector3 v3location = new Vector3 (x, 0, y);
		v3location += new Vector3 (0.5f, 0, 0.5f);

		switch (direction) {
		case 0:
			transformvector = new Vector3 (1, 0, 0);
			break;
		case 1:
			transformvector = new Vector3 (0, 0, 1);
			break;
		case 2:
			transformvector = new Vector3 (-1, 0, 0);
			break;
		case 3:
			transformvector = new Vector3 (0, 0, -1);
			break;
		default:
			Debug.Log (direction + " went to default");
			transformvector = new Vector3 (0, 0, 0);
			break;
		}
		transformvector *= .5f;
		myplane.transform.position = transformvector + v3location;
		myplane.transform.rotation = Quaternion.Euler (0, rotation, 0);
	}

	void BuildDeadEnd(int rotation, int direction, int x, int y){
		GameObject deadEnd = GameObject.Instantiate (DeadEndPrefab, WallsParent.transform) as GameObject;
		Vector3 v3location = new Vector3 (x + .5f, 0, y+.5f);
		deadEnd.transform.position = v3location;
		deadEnd.transform.rotation = Quaternion.Euler (0, rotation, 0);
	}

	void BuildCorner(int rotation, int direction, int x, int y){
		GameObject corner = GameObject.Instantiate (CornerInnerPrefab, WallsParent.transform) as GameObject;
		Vector3 v3location = new Vector3 (x + .5f, 0, y+.5f);
		corner.transform.position = v3location;
		corner.transform.rotation = Quaternion.Euler (0, rotation, 0);
	}


	void buildDoors (p2D center, Vector3 convCenter, GameObject thesedoors){
		int[] surrounding = null;
		foreach (Room myroom in puzzleRoomsDG) {
			if (myroom.getCenter ().Equals (center)) {
				surrounding = myroom.getDirectiondoors ();
				break;
			}
		}
			
//		Debug.Log (surrounding[0] + "." + surrounding[1] + "." + surrounding[2] + "." + surrounding[3] + " " + center);
		for (int i = 0; i < 4; i++) {
			if (surrounding [i] == 1) {
				Instantiate (WoodenDoors, convCenter, Quaternion.Euler (0, i * 90, 0), thesedoors.transform);
			}
		}
	}

	//Returns the surroundings in an array, where 0 is a floor and 1 is a wall/roof
	int[] getSurrounding2(int x, int y) {
		int[] surroundings= new int[4];
		surroundings [0] = (maze [x + 1, y]) ? 1 : 0;
		surroundings [1] = (maze [x, y + 1]) ? 1 : 0;
		surroundings [2] = (maze [x - 1, y]) ? 1 : 0;
		surroundings [3] = (maze [x, y - 1]) ? 1 : 0;
		return surroundings;

	}

	/// <summary>
	/// <para>Gets the surrounding tiles where 1 = floor and 0 = roof</para>
	/// <para>3  2  1</para>
	/// <para>4  X  0</para>
	/// <para>5  6  7</para>
	/// </summary>
	/// <returns>The surrounding in format:</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
    int[] getSurrounding8(int x, int y)
    {
        int[] surroundings = new int[8];
        surroundings[0] = (maze[x + 1, y]) ? 1 : 0;
        surroundings[1] = (maze[x + 1, y+1]) ? 1 : 0;
        surroundings[2] = (maze[x, y + 1]) ? 1 : 0;
        surroundings[3] = (maze[x - 1, y + 1]) ? 1 : 0;
        surroundings[4] = (maze[x - 1, y]) ? 1 : 0;
        surroundings[5] = (maze[x - 1, y-1]) ? 1 : 0;
        surroundings[6] = (maze[x, y - 1]) ? 1 : 0;
        surroundings[7] = (maze[x + 1, y - 1]) ? 1 : 0;
        return surroundings;
    }

    bool cornersRoom(int x, int y)
    {
        int[] surr = getSurrounding8(x,y);
        for (int i = 0; i < 8; i++)
        {
            if (i == 6)
            {
                if (surr[i] == 0 && surr[i + 1] == 0 && surr[0] == 0)
                    return true;
            } else if (i == 7)
            {
                if (surr[i] == 0 && surr[0] == 0 && surr[1] == 0)
                    return true;
            } else if(surr[i]==0 && surr[i+1]==0 && surr[i+2]==0) {
                return true;
            }
            
        }
        return false;
    }

    void populatePuzzles () {
		int random = 0;

		//Make beginningRoom
		random = Random.Range (0,puzzleCenters.Count);
		p2D beginning = puzzleCenters [random];
		startpoint = new Vector3 (beginning.getX () * 6 + 1, 0, beginning.getY () * 6 + 1);
		startpoint += new Vector3 (-4f, 0, -4f);
		Instantiate (BeginningRoom, startpoint, Quaternion.Euler(new Vector3(0,Random.Range(0,4)*90,0)), Dungeon.transform);

		//Remove from list
		List<p2D> temp1 = new List<p2D> ();
		temp1.Add (beginning);
		p2D.myRemove (puzzleCenters, temp1);

		//Make endRoom
		random = Random.Range (0,puzzleCenters.Count);
		p2D ending = puzzleCenters [random];
		endpoint = new Vector3 (ending.getX () * 6 + 1, 0, ending.getY () * 6 + 1);
		endpoint += new Vector3 (-4f, 0, -4f);
		Instantiate (EndingRoom, endpoint, Quaternion.Euler(new Vector3(0,Random.Range(0,4)*90,0)), Dungeon.transform);

		//Remove from list
		List<p2D> temp2 = new List<p2D> ();
		temp2.Add (ending);
		p2D.myRemove (puzzleCenters, temp2);

		sbPuzzles.Append ("\n\nPuzzles in this dungeon: ");

		foreach (p2D center in puzzleCenters) {
			Vector3 convCenter = new Vector3 (center.getX () * 6 + 1, 0, center.getY () * 6 + 1);
			convCenter += new Vector3 (-4f, 0, -4f);

			int x = DeterminePuzzleRoom ();

			GameObject thispuzzle = Instantiate (puzzleRooms [x], convCenter, Quaternion.identity, Dungeon.transform) as GameObject;
			GameObject thesedoors = Instantiate (PuzzleDoors, convCenter, Quaternion.identity, thispuzzle.transform) as GameObject;

			if (thispuzzle.GetComponent<myLever> () != null) {
				thispuzzle.GetComponent<myLever> ().Reread ();
			}

			thesedoors.GetComponent<puzzleDoors> ().RoomType = thispuzzle.name;
			buildDoors (center, convCenter, thesedoors);
			Instantiate (PuzzleMist, convCenter, Quaternion.identity, thispuzzle.transform);
		}

		sbPuzzles.Append ("\n\n");
		Debug.Log (sbPuzzles.ToString ());
	} 
		
	/// <summary>
	/// Gets the maze value, where 0 is a floor and 1 is a wall/roof
	/// </summary>
	/// <returns>The maze value.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="z">The z coordinate.</param>
    int getMazeValue(int x, int z){
        if (inBounds(x, z)){
            if (maze[x, z]) {
                return 0;
            }
        }    
        return 1;
    }
		

    bool inBounds(int x, int z){
        return 0<=x && x<mazeSize[0] && 0<=z && z<mazeSize[1];
    }

    bool ArrayEquals(int[] a, int[] b){
        return a[0] == b[0] && a[1] == b[1] && a[2] == b[2] && a[3] == b[3];
    }

    bool ArrayCorner(int[] a){
        if (a[0] == a[1]) return true;
        if (a[1] == a[2]) return true;
        if (a[2] == a[3]) return true;
        if (a[3] == a[0]) return true;
        return false;
    }

    void spawnChest(int x,int z)
    {
		float chance = 0;
		float multiplier = 0;
		if (isDeadEnd (x, z)) {
			chance = chance_chest_deadEnd;
			multiplier = 3;
		} else {
			chance = chance_chest_corridors;
			multiplier = 1;
		}

		for(int i = 0; i < multiplier; i++){
        float random = Random.value;
	        if (random < chance)
	        {
				GameObject chest_instance = Instantiate(chest, new Vector3(x*6 + Random.Range(1f, 5f), 0, z*6 + Random.Range(1f,5f)),
					Quaternion.Euler(new Vector3(-90f, Random.Range(0f, 360f), 0f)), ChestsParent.transform) as GameObject;
				chest_instance.GetComponent<Chest> ().SetUp (dungeonParameters, PickupsParent.transform);
	        }
		}
    }

	/// <summary>
	/// Check if maze position is a dead end
	/// </summary>
	/// <returns><c>true</c>, if dead end, <c>false</c> otherwise.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="z">The z coordinate.</param>
	bool isDeadEnd(int x, int z){
		//check if current spot is a floor
		if (getMazeValue (x, z) == 0) {
			int[] surroundings = getSurrounding2 (x, z);
			int counter = 0;
			for (int i = 0; i < surroundings.Length; i++) {
				if (surroundings [i] == 0) {
					counter++;
				}
			}
			if (counter == 3) {
				return true;
			}
		}
		return false;
	}

	void spawnParticles(float x, float z)
	{
		float random = Random.value;
		if (random < chance_particles) {
			GameObject particles = Instantiate (dungeonParticles[Random.Range(0, dungeonParticles.Length)],
				new Vector3 (x * 6 + Random.Range (0f, 6f), 1.3f, z * 6 + Random.Range (0f, 6f)),
				Quaternion.Euler (new Vector3 (0f, Random.Range (0f, 360f), 0f)), ParticlesParent.transform) as GameObject;
			GameManager.Instance.addHighQualityItem (particles);
			if (!GameManager.Instance.data.highQuality) {
				particles.SetActive (false);
			}
		}
	}

	void spawnWallTorch(float x, float z)
	{
		int[] surroundings = getSurrounding2 ((int)x, (int)z);
		if (surroundings [0] == 0 && surroundings [1] == 1 && surroundings [2] == 0 && surroundings [3] == 1) {
			GameObject walltorch = Instantiate (wallTorch, new Vector3 (x * 6f + 3f, 0, z * 6f + Random.Range(2f, 4f)), Quaternion.identity, WallTorchParent.transform) as GameObject;
			if (Random.value > .5) {
				walltorch.transform.eulerAngles = new Vector3 (0f, 90f, 0f);
			} else {
				walltorch.transform.eulerAngles = new Vector3 (0f, 270f, 0f);
			}
		} else if (surroundings [0] == 1 && surroundings [1] == 0 && surroundings [2] == 1 && surroundings [3] == 0) {
			GameObject walltorch = Instantiate (wallTorch, new Vector3 (x * 6f + Random.Range(2f, 4f), 0, z * 6f + 3f), Quaternion.identity, WallTorchParent.transform) as GameObject;
			if (Random.value > .5) {
				walltorch.transform.eulerAngles = new Vector3 (0f, 180f, 0f);
			}
		}
	
	}

    Quaternion randomQuaternion()
    {
        float random = Random.value;
        if (random < .25f) {
            return Quaternion.identity;
        } else if (random < .5f)  {
            return Quaternion.identity * Quaternion.Euler(0, 90, 0);
        } else if (random < .75f) {
            return Quaternion.identity * Quaternion.Euler(0, 180, 0);
        } else {
            return Quaternion.identity * Quaternion.Euler(0, -90, 0);
        }
    }
    
    void trap(int x, int y)
    {
		/*
		if (cornersRoom (x, y))
			Debug.Log ("Corner here");
            return;

*/

		if (!ArrayCorner (getSurrounding2 (x, y)) /*|| nest*/) {
			GameObject trapprefab = traps [DetermineTrap ()];
			//Debug.Log ("no corner");
			Quaternion rot;
			int[] a = getSurrounding2 (x, y);
			if (a [0] == 1) {
				rot = Quaternion.Euler (0, 90, 0);
			} else if (a [3] == 1) {
				rot = Quaternion.Euler (0, 180, 0);
			} else if (a [2] == 1) {
				rot = Quaternion.Euler (0, 90, 0);
			} else {
				rot = Quaternion.identity;
			}
			GameObject trap = GameObject.Instantiate (trapprefab, Traps.transform) as GameObject;
			trap.transform.position = new Vector3 (x * 6 + 3, 0, y * 6 + 3);
			trap.transform.rotation = rot;
		} else {
			//Debug.Log ("corner");
		}
    }

	/*
    GameObject trapStraight()
    {
        return trap_box;
    }
    */

	void createStartEndPoint() {
    	Instantiate(portal, startpoint, Quaternion.identity, Dungeon.transform);
		Instantiate(end_portal, endpoint, Quaternion.identity, Dungeon.transform);

    }      

	public Vector3 MovePlayersToStart () {
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject tmp in players) {
			tmp.transform.position = startpoint + new Vector3 (7, 0.5f, 0);
		}
		return startpoint;
	}
  
	public void importMaze(int[,] maze)
	{
        import_maze = new bool[mazeSize[0], mazeSize[1]];
        for (int i = 0; i < mazeSize[0]; i++) {
            for(int j = 0; j < mazeSize[1]; j++){
                import_maze[i,j] = (maze[i+1,j+1]==1);
            }
        }
        Debug.Log(print(import_maze));
    }


    public bool[,] getMaze()
    {
        return maze;
    }

    public string print(bool[,] maze)
    {
        string append = "";
        for (int x = 0; x < mazeSize[0]; x++)
        {
            for (int y = 0; y < mazeSize[1]; y++)
            {
                if (maze[x, y])
                {
                    append += "1";
                }
                else
                {
                    append += "_";
                }
                append += " ";
            }
            append += "\n";
        }
        return append;
    }

	void NormalizePuzzleRoomChances ()
	{
		StringBuilder sb2 = new StringBuilder ();
		sb2.Append ("Raw values puzzleRoom chances: \n");
		for (int i = 0; i < puzzleRoomSpawnChances.Count; i++) {
			sb2.Append (puzzleRoomSpawnChances [i] + " ");
		}
		float norm = 0;
		int size = puzzleRoomSpawnChances.Count;
		for (int i = 0; i < puzzleRoomSpawnChances.Count; i++) {
			norm += puzzleRoomSpawnChances [i];
		}
		float temp = 0;
		for (int i = 0; i < puzzleRoomSpawnChances.Count; i++) {
			puzzleRoomSpawnChances [i] = (puzzleRoomSpawnChances [i] / norm) + temp;
			temp = puzzleRoomSpawnChances [i];
		}
		sb2.Append ("\n\nNew values: ");
		for (int i = 0; i < puzzleRoomSpawnChances.Count; i++) {
			sb2.Append (puzzleRoomSpawnChances [i] + " ");
		}

		sb2.Append ("\n\n");
		//Debug.Log (sb2.ToString ());
	}

	void NormalizeTrapChances ()
	{
		StringBuilder sb2 = new StringBuilder ();
		sb2.Append ("Raw values trap chances: \n");
		for (int i = 0; i < trapSpawnChances.Count; i++) {
			sb2.Append (trapSpawnChances [i] + " ");
		}
		float norm = 0;
		int size = trapSpawnChances.Count;
		for (int i = 0; i < trapSpawnChances.Count; i++) {
			norm += trapSpawnChances [i];
		}
		float temp = 0;
		for (int i = 0; i < trapSpawnChances.Count; i++) {
			trapSpawnChances [i] = (trapSpawnChances [i] / norm) + temp;
			temp = trapSpawnChances [i];
		}
		sb2.Append ("\n\nNew values: ");
		for (int i = 0; i < trapSpawnChances.Count; i++) {
			sb2.Append (trapSpawnChances [i] + " ");
		}

		sb2.Append ("\n\n");
		//Debug.Log (sb2.ToString ());
	}

	int DeterminePuzzleRoom(){
		float x = Random.value;
		for (int i = 0; i < puzzleRoomSpawnChances.Count; i++) {
			if (puzzleRoomSpawnChances[i] >= x) {
				sbPuzzles.Append ("\n" + puzzleRooms [i].ToString ());
				return i;
			}
		}
		return 0;
	}

	int DetermineTrap(){
		float x = Random.value;
		for (int i = 0; i < trapSpawnChances.Count; i++) {
			if (trapSpawnChances[i] >= x) {
				sbTraps.Append ("\n" + traps [i].ToString ());
				return i;
			}
		}
		return 0;
	}
}
