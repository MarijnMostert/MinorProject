using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterGenerator : MonoBehaviour {

    GameObject floor, side, sideAlt1, sideAlt2, corner, cornerout,
               roof, block, trap_straight, trap_crossing, trap_box,
               portal, cam, pointer, scene_manager,
               player, event_system, game_manager, UI, spawner, score_manager;

    public int width;// = 100;
	public int height;// = 90;
	public int radius;// = 2;
	public int maxlength;// = 3;
	public int timeout;// = 200;
	public int minAmountOfRooms;// = 6;
	public int maxAmountOfRooms;// = 8;
	public int chanceOfRoom;// = 15;
	public int[,] endMaze;

	void Awake () {
		
	}

	// Use this for initialization
	void Start () {
		bool done = false;
		int donerooms = 0;

		DungeonGenerator dungeon = null;
		int[,] maze = new int[width, height];
		List<p2D> doors = new List<p2D> ();

        LoadPrefabs();
        DungeonInstantiate dungeon_instantiate = new DungeonInstantiate(floor, side, sideAlt1, sideAlt2, corner, cornerout,
                                                                        roof, block, trap_straight, trap_crossing, trap_box,
                                                                        portal, cam, UI, pointer, scene_manager,
                                                                        player, event_system, game_manager, spawner, 
                                                                        score_manager, new int[2] {width,height});
        while (!done) {
			dungeon = new DungeonGenerator ( width,
											height,
											radius,
											maxlength,
											timeout,
											minAmountOfRooms,
											maxAmountOfRooms,
											chanceOfRoom );
			maze = dungeon.getMaze ();
			doors = dungeon.getDoorways ();
			done = dungeon.isDone ();
			donerooms = dungeon.getRooms ().Count;
		}	

		this.endMaze = maze;

		Debug.Log(donerooms);		
		Debug.Log(maze);

		//Analytics analysis = new Analytics(dungeon);
		//Debug.Log(analysis.deadEnds());
		//analysis.cleanUpDeadEnds();

		Debug.Log(print(maze));
        dungeon_instantiate.importMaze(this.endMaze);
        dungeon_instantiate.createMaze();

	}

    public int[,] getMaze()
    {
        return endMaze;
    }

	public string print (int[,] maze) {
		string append = "";
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
                if (maze[x, y] == 1)
                {
                    append += maze[x, y];
                } else
                {
                    append += "_";
                }
				append += " ";
			}
			append += "\n";
		}
		return append;
	}

    void LoadPrefabs()
    {
        floor = Resources.Load("Blocks/floor", typeof(GameObject)) as GameObject;
        side = Resources.Load("Blocks/side", typeof(GameObject)) as GameObject;
        sideAlt1 = Resources.Load("Blocks/side_alt1", typeof(GameObject)) as GameObject;
        sideAlt2 = Resources.Load("Blocks/side_alt2", typeof(GameObject)) as GameObject;
        corner = Resources.Load("Blocks/corner", typeof(GameObject)) as GameObject;
        cornerout = Resources.Load("Blocks/cornerout", typeof(GameObject)) as GameObject;
        roof = Resources.Load("Blocks/Roof", typeof(GameObject)) as GameObject;
        block = Resources.Load("Blocks/box", typeof(GameObject)) as GameObject;
        trap_straight = Resources.Load("Blocks/trap_straight", typeof(GameObject)) as GameObject;
        trap_crossing = Resources.Load("Blocks/trap_crossing", typeof(GameObject)) as GameObject;
        trap_box = Resources.Load("Blocks/trap_box", typeof(GameObject)) as GameObject;
        portal = Resources.Load("Blocks/portal", typeof(GameObject)) as GameObject;
        scene_manager = Resources.Load("SceneManager", typeof(GameObject)) as GameObject;
        cam = Resources.Load("Camera", typeof(GameObject)) as GameObject;
        pointer = Resources.Load("Cursor pointer", typeof(GameObject)) as GameObject;
        player = Resources.Load("Player", typeof(GameObject)) as GameObject;
        event_system = Resources.Load("EventSystem", typeof(GameObject)) as GameObject;
        game_manager = Resources.Load("Game Manager", typeof(GameObject)) as GameObject;
        spawner = Resources.Load("Spawner", typeof(GameObject)) as GameObject;
        score_manager = Resources.Load("Score Manager", typeof(GameObject)) as GameObject;
        UI = Resources.Load("UI", typeof(GameObject)) as GameObject;
    }
}
