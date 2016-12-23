using UnityEngine;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

public class DungeonInstantiate : Object {
    GameObject floor, side, sideAlt1, sideAlt2, corner, cornerout,
                            roof, block, trap_straight, trap_crossing, 
                            trap_box, portal, end_portal, player, 
                            game_manager, spawner, torch, 
                            cam, pointer, chest, coin, fireball, 
                            iceball, health, roofGroup, wallTorch;
    GameObject[] starters_pack, chest_pack;
    //GameObject[,] dungeon;
    int[] mazeSize;
	bool[,] maze, import_maze, trapped;
    float chance_trap_straight, chance_trap_crossing,
          chance_side_alt1, chance_side_alt2, step,
          chance_chest;
    bool start_defined;
    int[] count = new int[2] {0,2};
	private GameObject Dungeon;

	List<GameObject> puzzleRooms;

	List<p2D> puzzleCoords;
	List<p2D> puzzleCenters;
	List<Room> puzzleRoomsDG;

	GameObject WallsParent;
	GameObject FloorsParent;
	GameObject RoofsParent;
	GameObject BeginningRoom;
	GameObject EndingRoom;

	Vector3 startpoint;
	Vector3 endpoint;

	GameObject RoofPrefab;
	GameObject WallPrefab;
	GameObject FloorPrefab;
	GameObject PuzzleMist;
	GameObject WoodenDoors;
	GameObject PuzzleDoors;

    public Vector3 startPos;

    // Use this for initialization
    public DungeonInstantiate(GameObject floor, GameObject side, GameObject sideAlt1, GameObject sideAlt2, GameObject corner, 
                            GameObject cornerout, GameObject roof, GameObject block, GameObject trap_straight, GameObject trap_crossing, 
                            GameObject trap_box, GameObject portal, GameObject end_portal, GameObject player, 
                            GameObject game_manager, GameObject spawner, GameObject torch, GameObject cam, GameObject pointer, 
		GameObject chest, GameObject coin, GameObject fireball, GameObject iceball, GameObject health, int[] mazeSize, GameObject laser, GameObject shieldPickUp,
		GameObject stickyPickUp, GameObject roofGroup, GameObject wallPickUp, List<GameObject> puzzleRooms, GameObject wallTorch, GameObject piercingWeapon)

    {
        this.floor = floor;
        this.side = side;
        this.sideAlt1 = sideAlt1;
        this.sideAlt2 = sideAlt2;
        this.corner = corner;
        this.cornerout = cornerout;
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
		this.chest_pack = new GameObject[] { coin, fireball, iceball, health, laser, shieldPickUp, stickyPickUp, wallPickUp, piercingWeapon};
        this.player = player;
        this.mazeSize = new int[2] { mazeSize[0] - 2, mazeSize[1] - 2 };
        this.spawner = spawner;
        this.game_manager = game_manager;
		this.puzzleRooms = puzzleRooms;
		this.puzzleCoords = new List<p2D> ();
		this.puzzleCenters = new List<p2D> ();
		this.puzzleRoomsDG = new List<Room> ();

		WallsParent = new GameObject("Walls");
		FloorsParent = new GameObject("Floors");
		RoofsParent = new GameObject("Roofs");

		RoofPrefab = Resources.Load ("Prefabs/Blocks/RoofPrefab", typeof(GameObject)) as GameObject;
		WallPrefab = Resources.Load ("Prefabs/Blocks/WallPrefab", typeof(GameObject)) as GameObject;
		FloorPrefab = Resources.Load ("Prefabs/Blocks/FloorPrefab", typeof(GameObject)) as GameObject;
		PuzzleMist = Resources.Load("Prefabs/PuzzlesScenes/PuzzleMist", typeof(GameObject)) as GameObject;
		WoodenDoors =  Resources.Load("Prefabs/PuzzlesScenes/WoodenDoor", typeof(GameObject)) as GameObject;
		PuzzleDoors = Resources.Load("Prefabs/PuzzlesScenes/PuzzleDoors", typeof(GameObject)) as GameObject;

		BeginningRoom = Resources.Load ("Prefabs/PuzzlesScenes/BeginningRoom", typeof(GameObject)) as GameObject;
		EndingRoom = Resources.Load ("Prefabs/PuzzlesScenes/EndingRoom", typeof(GameObject)) as GameObject;


		this.starters_pack = new GameObject[] {torch};
		this.roofGroup = roofGroup;
		this.wallTorch = wallTorch;

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
		chance_chest = 0.5f;

		//Instantiate empty Dungeon GameObject
		Dungeon = new GameObject("Dungeon");
		WallsParent.transform.SetParent(Dungeon.transform);
		FloorsParent.transform.SetParent(Dungeon.transform);
		RoofsParent.transform.SetParent(Dungeon.transform);

        //import starters pack
		InstantiateStarterPack(starters_pack, new Vector3(0, 0, 0),Quaternion.identity);
        //Instantiate(scene_manager, new Vector3(0, 0, 0), Quaternion.identity);

		spawner.GetComponent<Spawner>().mapMinX = 0;
        spawner.GetComponent<Spawner>().mapMinZ = 0;
        spawner.GetComponent<Spawner>().mapMaxX = mazeSize[0]*6;
        spawner.GetComponent<Spawner>().mapMaxZ = mazeSize[1]*6;
		spawner = Instantiate(spawner, new Vector3(0, 0, 0), Quaternion.identity, Dungeon.transform) as GameObject;

		GameObject.Find ("Game Manager").GetComponent<GameManager> ().spawner = spawner.GetComponent<Spawner> ();

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
		for (int x = 0; x < mazeSize[0]; x++) {
			for (int y = 0; y < mazeSize[1]; y++) {
				bool puzzle = false;
				if (p2D.myContains (puzzleCoords, new p2D (x+1, y+1))) {
					puzzle = true; } // sla over
				if (maze[x,y]) {
					buildOpen (x,y,puzzle);
				} 
				else {
					buildRoof (x, y);
				} 
			}
		}
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
			spawnChest (x,y);
			spawnWallTorch (x, y);
		}

		int[] arrayS = getSurrounding2(x, y);
		int rotation = 180;
		int direction = 0;
		string total = "";
		for (int i = 0; i < arrayS.Length ; i++) {
			total += arrayS[i].ToString();
			total += " ";
			if (arrayS[i] == 0) {
				BuildWall(rotation, direction%4, x, y);
			}
			rotation -= 90;
			direction++;
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
		myplane.transform.rotation = Quaternion.Euler (0, rotation, -90);
	}

	void buildDoors (p2D center, Vector3 convCenter, GameObject thesedoors){
		int[] surrounding = null;
		foreach (Room myroom in puzzleRoomsDG) {
			if (myroom.getCenter ().Equals (center)) {
				surrounding = myroom.getDirectiondoors ();
				break;
			}
		}
			
		Debug.Log (surrounding[0] + "." + surrounding[1] + "." + surrounding[2] + "." + surrounding[3] + " " + center);
		for (int i = 0; i < 4; i++) {
			if (surrounding [i] == 1) {
				Instantiate (WoodenDoors, convCenter, Quaternion.Euler (0, i * 90, 0), thesedoors.transform);
			}
		}
	}

	int[] getSurrounding2(int x, int y) {
		int[] surroundings= new int[4];
		surroundings [0] = (maze [x + 1, y]) ? 1 : 0;
		surroundings [1] = (maze [x, y + 1]) ? 1 : 0;
		surroundings [2] = (maze [x - 1, y]) ? 1 : 0;
		surroundings [3] = (maze [x, y - 1]) ? 1 : 0;
		return surroundings;
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

		foreach (p2D center in puzzleCenters) {
			Vector3 convCenter = new Vector3 (center.getX () * 6 + 1, 0, center.getY () * 6 + 1);
			convCenter += new Vector3 (-4f, 0, -4f);
			int number = Random.Range (0,puzzleRooms.Count);

			GameObject thispuzzle = Instantiate (puzzleRooms [number], convCenter, Quaternion.identity, Dungeon.transform) as GameObject;
			GameObject thesedoors = Instantiate (PuzzleDoors, convCenter, Quaternion.identity, thispuzzle.transform) as GameObject;

			if (thispuzzle.GetComponent<SwitchGround> () != null) {
				thispuzzle.GetComponent<SwitchGround> ().Reread ();
			}

			thesedoors.GetComponent<puzzleDoors> ().RoomType = thispuzzle.name;
			buildDoors (center, convCenter, thesedoors);
			Instantiate (PuzzleMist, convCenter, Quaternion.identity, thispuzzle.transform);
		}
	} 
		

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

    void spawnChest(float x,float z)
    {
        float random = Random.value;
        if (random < chance_chest)
        {
			GameObject chest_instance = Instantiate(chest, new Vector3(x*6 + Random.Range(1f, 5f), 0, z*6 + Random.Range(1f,5f)), Quaternion.identity, Dungeon.transform) as GameObject;
			chest_instance.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
			int number_of_items = Random.Range(0,4);
            for (int i = 0; i <= number_of_items; i++)
            {
				int item_number = Random.Range (0, chest_pack.Length);
                chest_instance.GetComponent<Chest>().addItem(chest_pack[item_number]);
            }
        }
    }

	void spawnWallTorch(float x, float z)
	{
		int[] surroundings = getSurrounding2 ((int)x, (int)z);
		if (surroundings [0] == 0 && surroundings [1] == 1 && surroundings [2] == 0 && surroundings [3] == 1) {
			GameObject walltorch = Instantiate (wallTorch, new Vector3 (x * 6f + 3f, 0, z * 6f + Random.Range(2f, 4f)), Quaternion.identity, Dungeon.transform) as GameObject;
			if (Random.value > .5) {
				walltorch.transform.eulerAngles = new Vector3 (0f, 90f, 0f);
			} else {
				walltorch.transform.eulerAngles = new Vector3 (0f, 270f, 0f);
			}
		} else if (surroundings [0] == 1 && surroundings [1] == 0 && surroundings [2] == 1 && surroundings [3] == 0) {
			GameObject walltorch = Instantiate (wallTorch, new Vector3 (x * 6f + Random.Range(2f, 4f), 0, z * 6f + 3f), Quaternion.identity, Dungeon.transform) as GameObject;
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

    GameObject trapStraight()
    {
        return trap_box;
    }

	void createStartEndPoint() {
    	GameObject start_GO = Instantiate(portal, startpoint, Quaternion.identity, Dungeon.transform) as GameObject;
		GameObject end_GO = Instantiate(end_portal, endpoint, Quaternion.identity, Dungeon.transform) as GameObject;

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

	void InstantiateStarterPack(GameObject[] starters_pack, Vector3 pos, Quaternion rot)
    {
		GameManager gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
        foreach(GameObject item in starters_pack)
        {
			GameObject temp = Instantiate(item, pos, rot, Dungeon.transform) as GameObject;
			if (temp.CompareTag ("Torch")) {
				gameManager.torch = temp.GetComponent<Torch> ();
			} /*else if (temp.CompareTag ("Camera")) {
				gameManager.mainCamera = temp.GetComponentInChildren<Camera> ();
			}
			*/
        }
       // game_manager.GetComponent<GameManager>().mainCamera = cam.GetComponentInChildren<Camera>() as Camera;
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
}
