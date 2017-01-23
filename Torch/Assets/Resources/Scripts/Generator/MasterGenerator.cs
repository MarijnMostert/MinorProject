using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterGenerator : Object {

    public GameObject floor, side, sideAlt1, sideAlt2, corner, cornerout,
                            roof, block, trap_straight, trap_crossing, trap_box,
                            portal, end_portal, player,
                            spawner, torch, cam, pointer, chest,
                            coin, fireball, iceball, health, laser, shieldPickUp,
							stickyPickUp, roofGroup, wallPickUp, wallTorch, piercingWeapon,
							bombPickUp, spidernest, wizardnest, wallspikes, spikes, shuriken, 
                            wallrush, stardustParticles, moondustParticles, decoyPickUp;
    GameObject game_manager;
	DungeonData.DungeonParameters dungeonParameters;
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

	public MasterGenerator(GameObject game_manager, DungeonData.DungeonParameters dungeonParameters, int radius, int maxlength, 
		int timeout)
    {
		this.dungeonParameters = dungeonParameters;
        this.game_manager = game_manager;
		this.width = dungeonParameters.width;
		this.height = dungeonParameters.height;
        this.radius = radius;
        this.maxlength = maxlength;
        this.timeout = timeout;
		this.minAmountOfRooms = dungeonParameters.minAmountOfRooms;
		this.maxAmountOfRooms = dungeonParameters.maxAmountOfRooms;
		this.chanceOfRoom = dungeonParameters.chanceOfRoom;

    }

	// Use this for initialization
	public void Constructing ()
    {
        bool done = false;
		int donerooms = 0;
		DungeonGenerator dungeon = null;
		int[,] maze = new int[width, height];
        int[] mazeSize = new int[2] {width, height};
        List<p2D> doors = new List<p2D> ();
		List<p2D> puzzleCenters = new List<p2D> ();
		List<p2D> allRoomCoords = new List<p2D> ();
		List<int[]> allDoors = new List<int[]> ();

        dungeon_instantiate = new DungeonInstantiate(dungeonParameters, floor, side, sideAlt1, sideAlt2, corner, cornerout,
                                                                        roof, block, trap_straight, trap_crossing, trap_box,
                                                                        portal, end_portal, player, game_manager, spawner, torch, 
                                                                        cam, pointer, chest, coin, fireball, iceball, health, mazeSize, 
                                                                        laser, shieldPickUp, stickyPickUp, roofGroup, wallPickUp,
                                                                        wallTorch, piercingWeapon, bombPickUp, spidernest, wizardnest, wallspikes, 
                                                                        spikes, shuriken, wallrush, stardustParticles, moondustParticles, decoyPickUp);


		while (!done) {
			dungeon = new DungeonGenerator ( width,
											height,
											radius,
											maxlength,
											timeout,
											minAmountOfRooms,
											maxAmountOfRooms,
											chanceOfRoom ); 
			done = dungeon.isDone ();
		}

		maze = dungeon.getMaze ();
		doors = dungeon.getDoorways ();
		donerooms = dungeon.getRooms ().Count;
		allRoomCoords = dungeon.getAllRoomCoords ();
		puzzleCenters = dungeon.getRoomCenters ();

		List<Room> allrooms = dungeon.getRooms ();

		GameManager.Instance.requiredCollectedKeys = allrooms.Count - 2;

        this.endMaze = maze;

		//Debug.Log(donerooms);		
		//Debug.Log(maze);

		//Analytics analysis = new Analytics(dungeon);
		//Debug.Log(analysis.deadEnds());
		//analysis.cleanUpDeadEnds();

		dungeon_instantiate.setPuzzleCoords (allRoomCoords);
		dungeon_instantiate.setPuzzleCenters (puzzleCenters);
		dungeon_instantiate.setPuzzleRoomsDG (allrooms);

		Debug.Log(print(maze));
        dungeon_instantiate.importMaze(this.endMaze);

        dungeon_instantiate.createMaze();

        Debug.Log(dungeon_instantiate.print(dungeon_instantiate.getMaze()));
        //spawner.GetComponent<Spawner>().importMaze(dungeon_instantiate.getMaze(),mazeSize);

    }

	public Vector3 MovePlayersToStart () {
		return dungeon_instantiate.MovePlayersToStart ();
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
		cam.GetComponentInChildren<CameraController>().gameManager = game_manager.GetComponent<GameManager>();
        spawner = Resources.Load("Prefabs/Spawner", typeof(GameObject)) as GameObject;
//        spawner.GetComponent<Spawner>().mapMinX = 5;
        player = Resources.Load("Prefabs/Player", typeof(GameObject)) as GameObject;
        torch = Resources.Load("Prefabs/Torch", typeof(GameObject)) as GameObject;
        fireball = Resources.Load("Prefabs/PickUps/FireBall Weapon PickUp", typeof(GameObject)) as GameObject;
        iceball = Resources.Load("Prefabs/PickUps/IceBall Weapon PickUp", typeof(GameObject)) as GameObject;
		laser = Resources.Load ("Prefabs/PickUps/Laser Weapon PickUp", typeof(GameObject)) as GameObject;

		roofGroup = Resources.Load ("Prefabs/roofGroup", typeof(GameObject)) as GameObject;
		wallPickUp = Resources.Load ("Prefabs/PickUps/Wall PickUp", typeof(GameObject)) as GameObject;
		wallTorch = Resources.Load ("Prefabs/WallTorch", typeof(GameObject)) as GameObject;
		piercingWeapon = Resources.Load ("Prefabs/PickUps/Piercing Weapon PickUp", typeof(GameObject)) as GameObject;

        wizardnest = Resources.Load("Prefabs/traps/nests/wizardnest/wizardsnest", typeof(GameObject)) as GameObject;
        spidernest = Resources.Load("Prefabs/traps/nests/spidernest/spidernest", typeof(GameObject)) as GameObject;
        wallrush = Resources.Load("Prefabs/traps/dungeon/wallrush/WallRushPrefab", typeof(GameObject)) as GameObject;
        shuriken = Resources.Load("Prefabs/traps/dungeon/shuriken/ShurikenPrefab", typeof(GameObject)) as GameObject;
        spikes = Resources.Load("Prefabs/traps/dungeon/spikes/SpikesPrefab", typeof(GameObject)) as GameObject;
        wallspikes = Resources.Load("Prefabs/traps/dungeon/wallspikes/WallSpikePrefab", typeof(GameObject)) as GameObject;
        stardustParticles = Resources.Load ("Prefabs/Stardust Particles", typeof(GameObject)) as GameObject;
		moondustParticles = Resources.Load ("Prefabs/Moondust Particles", typeof(GameObject)) as GameObject;
    }
}
