using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterGenerator : Object {

    public GameObject floor, side, sideAlt1, sideAlt2, corner, cornerout,
                            roof, block, trap_straight, trap_crossing, trap_box,
                            portal, end_portal, player, pause_screen,
                            spawner, torch, cam, ui, pointer, chest,
                            coin, fireball, iceball, health, laser, shieldPickUp,
							stickyPickUp;
    GameObject game_manager;
    public DungeonInstantiate dungeon_instantiate;
    int width;// = 100;
	int height;// = 90;
	int radius;// = 2;
	int maxlength;// = 3;
	int timeout;// = 200;
	int minAmountOfRooms;// = 6;
	int maxAmountOfRooms;// = 8;
	int chanceOfRoom;// = 15;

	int[,] endMaze;

    public MasterGenerator(GameObject game_manager, int width, int height, int radius, int maxlength, 
                            int timeout, int minAmountOfRooms, int maxAmountOfRooms, int chanceOfRoom)
    {
        this.game_manager = game_manager;
        this.width = width;
        this.height = height;
        this.radius = radius;
        this.maxlength = maxlength;
        this.timeout = timeout;
        this.minAmountOfRooms = minAmountOfRooms;
        this.maxAmountOfRooms = maxAmountOfRooms;
        this.chanceOfRoom = chanceOfRoom;
    }

	// Use this for initialization
	public void Start ()
    {
        bool done = false;
		int donerooms = 0;
		DungeonGenerator dungeon = null;
		int[,] maze = new int[width, height];
        int[] mazeSize = new int[2] {width, height};
        List<p2D> doors = new List<p2D> ();

        dungeon_instantiate = new DungeonInstantiate(floor, side, sideAlt1, sideAlt2, corner, cornerout,
                                                                        roof, block, trap_straight, trap_crossing, trap_box,
                                                                        portal, end_portal, player, pause_screen, game_manager,
                                                                        spawner, torch, cam, ui, pointer, chest, coin, 
																		fireball, iceball, health, mazeSize, laser, shieldPickUp,
																		stickyPickUp);

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

		//Debug.Log(donerooms);		
		//Debug.Log(maze);

		//Analytics analysis = new Analytics(dungeon);
		//Debug.Log(analysis.deadEnds());
		//analysis.cleanUpDeadEnds();

		Debug.Log(print(maze));
        dungeon_instantiate.importMaze(this.endMaze);
        dungeon_instantiate.createMaze();

        Debug.Log(dungeon_instantiate.print(dungeon_instantiate.getMaze()));
        //spawner.GetComponent<Spawner>().importMaze(dungeon_instantiate.getMaze(),mazeSize);

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

    public void LoadPrefabs()
    {
        floor = Resources.Load("Prefabs/Blocks/floor", typeof(GameObject)) as GameObject;
        side = Resources.Load("Prefabs/Blocks/side", typeof(GameObject)) as GameObject;
        sideAlt1 = Resources.Load("Prefabs/Blocks/side_alt1", typeof(GameObject)) as GameObject;
        sideAlt2 = Resources.Load("Prefabs/Blocks/side_alt2", typeof(GameObject)) as GameObject;
        corner = Resources.Load("Prefabs/Blocks/corner", typeof(GameObject)) as GameObject;
        cornerout = Resources.Load("Prefabs/Blocks/cornerout", typeof(GameObject)) as GameObject;
        roof = Resources.Load("Prefabs/Blocks/Roof", typeof(GameObject)) as GameObject;
        block = Resources.Load("Prefabs/Blocks/box", typeof(GameObject)) as GameObject;
        trap_straight = Resources.Load("Prefabs/Blocks/trap_straight", typeof(GameObject)) as GameObject;
        trap_crossing = Resources.Load("Prefabs/Blocks/trap_crossing", typeof(GameObject)) as GameObject;
        trap_box = Resources.Load("Prefabs/Blocks/trap_box", typeof(GameObject)) as GameObject;
        portal = Resources.Load("Prefabs/Blocks/portal", typeof(GameObject)) as GameObject;
        end_portal = Resources.Load("Prefabs/Blocks/endPortal", typeof(GameObject)) as GameObject;
        chest = Resources.Load("Prefabs/chest", typeof(GameObject)) as GameObject;
        cam = Resources.Load("Prefabs/Camera", typeof(GameObject)) as GameObject;
        cam.GetComponentInChildren<CameraController>().gameManager = game_manager;
        pause_screen = Resources.Load("Prefabs/Pause Screen", typeof(GameObject)) as GameObject;
        pause_screen.GetComponentInChildren<MuteAudio>().game_manager = game_manager;
        ui = Resources.Load("Prefabs/UI", typeof(GameObject)) as GameObject;
        spawner = Resources.Load("Prefabs/Spawner", typeof(GameObject)) as GameObject;
        spawner.GetComponent<Spawner>().mapMinX = 5;
        player = Resources.Load("Prefabs/Player", typeof(GameObject)) as GameObject;
        torch = Resources.Load("Prefabs/UI", typeof(GameObject)) as GameObject;
        coin = Resources.Load("Prefabs/PickUps/Coin", typeof(GameObject)) as GameObject;
        coin.GetComponent<ScorePickUp>().gameManager = game_manager;
        fireball = Resources.Load("Prefabs/PickUps/FireBall Weapon PickUp", typeof(GameObject)) as GameObject;
        iceball = Resources.Load("Prefabs/PickUps/IceBall Weapon PickUp", typeof(GameObject)) as GameObject;
        health = Resources.Load("Prefabs/PickUps/HealthPickUp", typeof(GameObject)) as GameObject;
		laser = Resources.Load ("Prefabs/PickUps/Laser Weapon PickUp", typeof(GameObject)) as GameObject;
		shieldPickUp = Resources.Load ("Prefabs/PickUps/Shield PickUp", typeof(GameObject)) as GameObject;
		stickyPickUp = Resources.Load ("Prefabs/PickUps/Sticky PickUp", typeof(GameObject)) as GameObject;

    }
}
