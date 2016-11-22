using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterGenerator : MonoBehaviour {

    public GameObject floor, side, sideAlt1, sideAlt2, corner, cornerout,
                            roof, block, trap_straight, trap_crossing, trap_box,
                            portal, cam, ui, pointer, starters_pack, scene_manager;

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

        DungeonInstantiate dungeon_instantiate = new DungeonInstantiate(floor, side, sideAlt1, sideAlt2, corner, cornerout,
                                                                        roof, block, trap_straight, trap_crossing, trap_box,
                                                                        portal, cam, ui, pointer, starters_pack, scene_manager,
                                                                        new int[2] {width,height});

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

}
