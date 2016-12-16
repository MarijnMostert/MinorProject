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
                            iceball, health, roofGroup;
    GameObject[] starters_pack, chest_pack;
    GameObject[,] dungeon;
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

	GameObject WallsParent;
	GameObject FloorsParent;
	GameObject RoofsParent;
	GameObject WallObject;
	GameObject BeginningRoom;
	GameObject EndingRoom;

	Material RoofMaterial;

    public Vector3 startPos;

    // Use this for initialization
    public DungeonInstantiate(GameObject floor, GameObject side, GameObject sideAlt1, GameObject sideAlt2, GameObject corner, 
                            GameObject cornerout, GameObject roof, GameObject block, GameObject trap_straight, GameObject trap_crossing, 
                            GameObject trap_box, GameObject portal, GameObject end_portal, GameObject player, 
                            GameObject game_manager, GameObject spawner, GameObject torch, GameObject cam, GameObject pointer, 
		GameObject chest, GameObject coin, GameObject fireball, GameObject iceball, GameObject health, int[] mazeSize, GameObject laser, GameObject shieldPickUp,
	GameObject stickyPickUp, GameObject roofGroup, GameObject wallPickUp, List<GameObject> puzzleRooms)

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
		this.chest_pack = new GameObject[] { coin, fireball, iceball, health, laser, shieldPickUp, stickyPickUp, wallPickUp};
        this.player = player;
        this.mazeSize = new int[2] { mazeSize[0] - 2, mazeSize[1] - 2 };
        this.spawner = spawner;
        this.game_manager = game_manager;

		this.puzzleRooms = puzzleRooms;
		this.puzzleCoords = new List<p2D> ();
		this.puzzleCenters = new List<p2D> ();
		WallsParent = GameObject.Find ("Walls");
		FloorsParent = GameObject.Find ("Floors");
		RoofsParent = GameObject.Find ("Roofs");

		RoofMaterial = Resources.Load("Materials/RoofMaterial", typeof(Material)) as Material;
		Debug.Log (RoofMaterial);

		WallObject = Resources.Load ("Prefabs/Blocks/CubeWall", typeof(GameObject)) as GameObject;

		BeginningRoom = Resources.Load ("Prefabs/PuzzlesScenes/BeginningRoom", typeof(GameObject)) as GameObject;
		EndingRoom = Resources.Load ("Prefabs/PuzzlesScenes/EndingRoom", typeof(GameObject)) as GameObject;


		this.starters_pack = new GameObject[] {torch, cam};
		this.roofGroup = roofGroup;

    }

	public void setPuzzleCoords (List<p2D> coordList) {
		puzzleCoords = coordList;
	}

	public void setPuzzleCenters (List<p2D> coordList) {
		puzzleCenters = coordList;
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
		chance_chest = 0.04f;

		

		//Instantiate empty Dungeon GameObject
		Dungeon = new GameObject("Dungeon");

        //import starters pack
		InstantiateStarterPack(starters_pack, new Vector3(0, 0, 0),Quaternion.identity);
        //Instantiate(scene_manager, new Vector3(0, 0, 0), Quaternion.identity);

        spawner.GetComponent<Spawner>().mapMinX = 5;
        spawner.GetComponent<Spawner>().mapMinZ = 5;
        spawner.GetComponent<Spawner>().mapMaxX = (mazeSize[0]-1)*2*3+5;
        spawner.GetComponent<Spawner>().mapMaxZ = (mazeSize[1] - 1) * 2 * 3 + 5;
		spawner = Instantiate(spawner, new Vector3(0, 0, 0), Quaternion.identity, Dungeon.transform) as GameObject;
		GameObject.Find ("Game Manager").GetComponent<GameManager> ().spawner = spawner.GetComponent<Spawner> ();

        mazeSize = new int[] { mazeSize[0], mazeSize[1]};
        //dungeon = new GameObject[mazeSize[0], mazeSize[1]];
        maze = new bool[mazeSize[0], mazeSize[1]];
        maze = import_maze;
		populteMaze2 ();
		populatePuzzles ();


		WallsParent.transform.localScale = new Vector3 (6, 1, 6);
		FloorsParent.transform.localScale = new Vector3 (6, 1, 6);
		RoofsParent.transform.localScale = new Vector3 (6, 1, 6);

        createStartEndPoint();
        this.spawner.GetComponent<Spawner>().importMaze(maze, mazeSize);
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
		GameObject myplane = GameObject.CreatePrimitive (PrimitiveType.Plane);
		myplane.transform.localScale = new Vector3 (0.1f, 1, 0.1f);
		myplane.transform.position = new Vector3 (x+0.5f, 2, y+0.5f);
		myplane.transform.SetParent(RoofsParent.transform);
		MeshRenderer meshr = myplane.GetComponent <MeshRenderer> () as MeshRenderer;
		meshr.material = RoofMaterial;

		//Bouw navmeshblokkade
	}

	void buildOpen(int x, int y, bool puzzle) {
		if (!puzzle) {
			GameObject myplane = GameObject.CreatePrimitive (PrimitiveType.Plane);
			myplane.transform.localScale = new Vector3 (0.1f, 1, 0.1f);
			myplane.transform.position = new Vector3 (x + 0.5f, 0, y + 0.5f);
			myplane.transform.SetParent (FloorsParent.transform);

			spawnChest (x,y);
		}

		int[] arrayS = getSurrounding2(x, y);
		int rotation = 90;
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
		GameObject myplane = GameObject.Instantiate (WallObject);

		//myplane.transform.localScale = new Vector3 (0.1f, 0, 0.6f);
		Vector3 transformvector;
		Vector3 v3location = new Vector3 (x, -1, y);
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
			Debug.Log (direction + " ẅent to default");
			transformvector = new Vector3 (0, 0, 0);
			break;
		}
		transformvector *= .5f;
		myplane.transform.position = transformvector + v3location;
		myplane.transform.rotation = Quaternion.Euler (0, rotation, 0);

		myplane.transform.SetParent(WallsParent.transform);
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
		Vector3 beginningCenter = new Vector3 (beginning.getX () * 6 + 1, 0, beginning.getY () * 6 + 1);
		beginningCenter += new Vector3 (-4f, 0, -4f);
		Instantiate (BeginningRoom, beginningCenter, Quaternion.Euler(new Vector3(0,Random.Range(0,4)*90,0)));

		List<p2D> temp1 = new List<p2D> ();
		temp1.Add (beginning);
		p2D.myRemove (puzzleCenters, temp1);

		//Make endRoom
		random = Random.Range (0,puzzleCenters.Count);
		p2D ending = puzzleCenters [random];
		Vector3 endCenter = new Vector3 (ending.getX () * 6 + 1, 0, ending.getY () * 6 + 1);
		endCenter += new Vector3 (-4f, 0, -4f);
		Instantiate (EndingRoom, endCenter, Quaternion.Euler(new Vector3(0,Random.Range(0,4)*90,0)));

		List<p2D> temp2 = new List<p2D> ();
		temp2.Add (ending);
		p2D.myRemove (puzzleCenters, temp2);

		foreach (p2D center in puzzleCenters) {
			Vector3 convCenter = new Vector3 (center.getX () * 6 + 1, 0, center.getY () * 6 + 1);
			convCenter += new Vector3 (-4f, 0, -4f);
			int number = Random.Range (0,puzzleRooms.Count);
			Instantiate (puzzleRooms [number], convCenter, Quaternion.identity);
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
			GameObject chest_instance = Instantiate(chest, new Vector3(x * step, -1, z * step), randomQuaternion(), Dungeon.transform) as GameObject;
            int number_of_items = Random.Range(0,4);
            for (int i = 0; i <= number_of_items; i++)
            {
				int item_number = Random.Range (0, chest_pack.Length);
                chest_instance.GetComponent<Chest>().addItem(chest_pack[item_number]);
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

    void createStartEndPoint()
    {
		//Kies een beginmuur 
        //GameObject start_GO = Instantiate(portal, new Vector3(step * start_coor[0], -1, step * start_coor[1]), findRot(type, surroundings)) as GameObject;

		//Kies een begin
        //GameObject end_GO = Instantiate(end_portal, new Vector3(step * end_coor[0], 1, step * end_coor[1]), findRot(end_type, end_surroundings)) as GameObject;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject tmp in players)
        {
            tmp.transform.position = new Vector3(115f,0.75f,115f);
        }
      //  startPos = new Vector3(step * start_coor[0], -.7f, step * start_coor[1]);

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
        foreach(GameObject item in starters_pack)
        {
			GameObject temp = Instantiate(item, pos, rot, Dungeon.transform) as GameObject;
			if (temp.CompareTag ("Torch")) {
				GameManager gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
				gameManager.torch = temp.GetComponent<Torch> ();
			}
        }
        game_manager.GetComponent<GameManager>().mainCamera = cam.GetComponentInChildren<Camera>() as Camera;
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
