using UnityEngine;
using System.Threading;
using System.Collections;

public class DungeonInstantiate : Object {
    GameObject floor, side, sideAlt1, sideAlt2, corner, cornerout,
                            roof, block, trap_straight, trap_crossing, 
                            trap_box, portal, end_portal, player, 
                            pause_screen, game_manager, spawner, torch, 
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

    public Vector3 startPos;

    // Use this for initialization
    public DungeonInstantiate(GameObject floor, GameObject side, GameObject sideAlt1, GameObject sideAlt2, GameObject corner, 
                            GameObject cornerout, GameObject roof, GameObject block, GameObject trap_straight, GameObject trap_crossing, 
                            GameObject trap_box, GameObject portal, GameObject end_portal, GameObject player, GameObject pause_screen, 
                            GameObject game_manager, GameObject spawner, GameObject torch, GameObject cam, GameObject pointer, 
		GameObject chest, GameObject coin, GameObject fireball, GameObject iceball, GameObject health, int[] mazeSize, GameObject laser, GameObject shieldPickUp,
		GameObject stickyPickUp, GameObject roofGroup, GameObject wallPickUp)
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
        this.starters_pack = new GameObject[] { pause_screen, torch, cam};
		this.roofGroup = roofGroup;
    }

    public void createMaze(){

        chance_trap_straight = 1f;
        chance_trap_crossing = 1f;
        chance_side_alt1 = 0.2f;
        chance_side_alt2 = 0.2f + chance_side_alt1;
        chance_chest = 0.04f;
        start_defined = false;
        step = 2f;

        //import starters pack
        InstantiateStarterPack(starters_pack, new Vector3(0, 0, 0),Quaternion.identity);
        //Instantiate(scene_manager, new Vector3(0, 0, 0), Quaternion.identity);

        spawner.GetComponent<Spawner>().mapMinX = 5;
        spawner.GetComponent<Spawner>().mapMinZ = 5;
        spawner.GetComponent<Spawner>().mapMaxX = (mazeSize[0]-1)*2*3+5;
        spawner.GetComponent<Spawner>().mapMaxZ = (mazeSize[1] - 1) * 2 * 3 + 5;
        spawner = Instantiate(spawner, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

        //simulate mazecreation
        /*import_maze = new bool[5, 5] {  {false,false,true,false,false},
                                        {false,false,true,false,false},
                                        {true,true,true,true,true},
                                        {false,false,true,false,false},
                                        {false,false,true,false,false} };
        printMaze(import_maze);*/

        updateProgress(0.2f);
        mazeSize = new int[] { mazeSize[0] * 3, mazeSize[1] * 3 };
        dungeon = new GameObject[mazeSize[0], mazeSize[1]];
        maze = new bool[mazeSize[0], mazeSize[1]];
        maze = StretchMatrix(import_maze);
        populateMaze();
        createStartEndPoint();
        this.spawner.GetComponent<Spawner>().importMaze(maze, mazeSize);
    }

    void populateMaze()
    {	
		GameObject dungeonEnvironment = Instantiate (new GameObject ());
		dungeonEnvironment.name = "Dungeon Environment";
		roofGroup = Instantiate(roofGroup, dungeonEnvironment.transform) as GameObject;
		GameObject floors = Instantiate (new GameObject (), dungeonEnvironment.transform) as GameObject;
		floors.name = "Floors";
		GameObject sides = Instantiate (new GameObject (), dungeonEnvironment.transform) as GameObject;
		sides.name = "Sides";
		GameObject corners = Instantiate (new GameObject (), dungeonEnvironment.transform) as GameObject;
		corners.name = "Corners";
        float deltaprogress = 0.5f / (mazeSize[0] * mazeSize[1]);
        for (int i = 0; i < mazeSize[0]; i++)
        {
            for (int j = 0; j < mazeSize[1]; j++)
            {
				if (maze [i, j]) {
					int[] surroundings = getSurroundings (i, j);
					int type = getSum (surroundings);
					switch (type) {
					case 0:
						dungeon [i, j] = Instantiate (chooseFloor (i, j), new Vector3 (step * i, 0, step * j), findRotFloor (i, j), floors.transform) as GameObject;
						spawnChest (i, j);
						break;
					case 1:
						dungeon [i, j] = Instantiate (chooseSide (), new Vector3 (step * i, 0, step * j), findRot (type, surroundings), sides.transform) as GameObject;
						spawnChest (i, j);
						break;
					case 2:
						dungeon [i, j] = Instantiate (corner, new Vector3 (step * i, 0, step * j), findRot (type, surroundings), corners.transform) as GameObject;
						spawnChest (i, j);
						break;
					case 3:
						dungeon [i, j] = Instantiate (cornerout, new Vector3 (step * i, 0, step * j), findRot (type, surroundings), corners.transform) as GameObject;
						break;
					default:
						break;
					}
					updateProgress (deltaprogress);
				} else {
					dungeon [i, j] = Instantiate (roof, new Vector3 (step * i, 0, step * j), Quaternion.Euler (new Vector3 (-90, 0, 0)), roofGroup.transform) as GameObject;
					if (getSum (getSurroundings (i, j)) == 4 && getSum (getDiagSurroundings (i, j)) == 4) {
						dungeon [i, j].GetComponent<NavMeshObstacle> ().enabled = false;
					}
				}
            }
        }
    }

    Quaternion findRot(int type, int[] surroundings){
        switch (type){
            //  case 0:
                //return findRotFloor(surroundings);
            case 1:
                return findRotSide(surroundings);
            case 2:
                return findRotCorner(surroundings);
            default:
                return findRotCorner2(surroundings);
        } 
    }

    Quaternion findRotFloor(int x, int z)
    {
        int[] surroundings = getSurDists(x, z);
        int[] diagSurroundings = getDiagSurDists(x, z);
        if (getSum(surroundings) == 2 && getSum(diagSurroundings) == 4)
        {
            if (surroundings[0] == 1 && surroundings[2] == 1)
            {
                return Quaternion.Euler(new Vector3(-90, 0, 0));
            }
            return Quaternion.Euler(new Vector3(-90, 90, 0));
        }
        return Quaternion.Euler(new Vector3(-90,0,0));
    }

    Quaternion findRotSide(int[] surroundings) {

        if(ArrayEquals(surroundings,new int[] {1,0,0,0})){
            return Quaternion.Euler(new Vector3(-90, 180, 0));
        }
        else if (ArrayEquals(surroundings,new int[] {0,1,0,0})){
            return Quaternion.Euler(new Vector3(-90, 90, 0));
        }
        else if (ArrayEquals(surroundings,new int[] {0,0,1,0})){
            return Quaternion.Euler(new Vector3(-90, 0, 0));
        }
        else {
            return Quaternion.Euler(new Vector3(-90, -90, 0));
        }
    }

    Quaternion findRotCorner(int[] surroundings){
        if (ArrayEquals(surroundings,new int[] {1,1,0,0})){
            return Quaternion.Euler(new Vector3(-90, 180, 0));
        }
        else if (ArrayEquals(surroundings,new int[] {0,1,1,0})){
            return Quaternion.Euler(new Vector3(-90, 90, 0));
        }
        else if (ArrayEquals(surroundings,new int[] {0,0,1,1})){
            return Quaternion.Euler(new Vector3(-90, 0, 0));
        }
        else {
            return Quaternion.Euler(new Vector3(-90, -90, 0));
        }
    }

    Quaternion findRotCorner2(int[] surroundings) {
        if (surroundings[0] == 1) {
            return Quaternion.Euler(new Vector3(-90, 90, 0));
        }
        else if (surroundings[1] == 1) {
            return Quaternion.Euler(new Vector3(-90, 180, 0));
        }
        else if (surroundings[2] == 1) {
            return Quaternion.Euler(new Vector3(-90, -90, 0));
        }
        else if (surroundings[3] == 1) {
            return Quaternion.Euler(new Vector3(-90, 0, 0));
        } else {
            return Quaternion.Euler(new Vector3(-90, 0, 90));
        }
    }

    int[] getSurroundings(int x, int z) {
        int[] surroundings= new int[4];
        surroundings[0] = getMazeValue(x+1, z);
        surroundings[1] = getMazeValue(x, z+1);
        surroundings[2] = getMazeValue(x-1, z);
        surroundings[3] = getMazeValue(x, z-1);
        return surroundings;
    }

	int[] getDiagSurroundings(int x, int z) {
		int[] surroundings= new int[4];
		surroundings[0] = getMazeValue(x+1, z+1);
		surroundings[1] = getMazeValue(x+1, z-1);
		surroundings[2] = getMazeValue(x-1, z+1);
		surroundings[3] = getMazeValue(x-1, z-1);
		return surroundings;
	}

    int[] getSurDists(int x, int z)
    {
        int[] surroundings = new int[4];
        surroundings[0] = getMazeValue(x + 3, z);
        surroundings[1] = getMazeValue(x, z + 3);
        surroundings[2] = getMazeValue(x - 3, z);
        surroundings[3] = getMazeValue(x, z - 3);
        return surroundings;
    }

    int[] getDiagSurDists(int x, int z)
    {
        int[] surroundings = new int[4];
        surroundings[0] = getMazeValue(x + 3, z + 3);
        surroundings[1] = getMazeValue(x + 3, z - 3);
        surroundings[2] = getMazeValue(x - 3, z + 3);
        surroundings[3] = getMazeValue(x - 3, z - 3);
        return surroundings;
    }

    int getSum(int[] array){
        int sum = 0;
        foreach (int tmp in array){
            sum += tmp;
        }
        return sum;
    }

    int getSum2Array(bool[,] array)
    {
        int sum = 0;
        foreach (bool tmp in array)
        {
            if (tmp)
            {
                sum ++;
            }
        }
        return sum;
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

    bool[,] StretchMatrix(bool[,] matrix)
    {
        bool[,] newMatrix = new bool[mazeSize[0], mazeSize[1]];
        for (int i = 0; i < mazeSize[0]/3; i++) {
            for (int j = 0; j < mazeSize[1]/3; j++) {
                for (int k = 0; k < 3; k++) {
                    for (int l = 0; l < 3; l++) {
                        newMatrix[3*i + k, 3*j + l] = matrix[i, j];
                    }
                }
            }
        }
        return newMatrix;
    }

    GameObject chooseSide()
    {
        float random = Random.value;
        if (random < chance_side_alt1)
        {
            return sideAlt1;
        }
        else if (random < chance_side_alt2)
        {
            return sideAlt2;
        }
        return side;
    }

    GameObject chooseFloor(int x, int z)
    {
        if ((x + 2) % 3 == 0 && (z + 2) % 3 == 0)
        {
            int[] surroundings = getSurDists(x, z);
            bool opp = (surroundings[0] == 0 && surroundings[2] == 0) || (surroundings[1] == 0 && surroundings[3] == 0);
            int sum = getSum(surroundings);
            int diagsum = getSum(getDiagSurDists(x, z));
            if (sum == 0 && diagsum == 4)
            {
                float random = Random.value;
                if (random < chance_trap_crossing)
                {
                    return trap_crossing;
                }
            }
            else if (sum == 2 && diagsum == 4 && opp)
            {
                count[0]++;
                if (count[0] == count[1])
                {
                    float random = Random.value;
                    if (random < chance_trap_straight)
                    {
                        return trapStraight();
                    }
                }
            }
        }

        return floor;
    }

    void spawnChest(float x,float z)
    {
        float random = Random.value;
        if (random < chance_chest)
        {
            GameObject chest_instance = Instantiate(chest, new Vector3(x * step, -1, z * step), randomQuaternion()) as GameObject;
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

    void updateProgress(float percentage)
    {
        //GameObject.Find("progress").GetComponent<progress>().updateProgress(percentage);
    }

    void createStartEndPoint()
    {
        int[] start_coor = pickRandomCoor();
        int[] surroundings = getSurroundings(start_coor[0], start_coor[1]);
        int type = getSum(surroundings);

        GameObject start_GO = Instantiate(portal, new Vector3(step * start_coor[0], -1, step * start_coor[1]), findRot(type, surroundings)) as GameObject;
        start_GO.transform.Translate(Vector3.forward);

        int[] end_coor = pickRandomCoor();
        int[] end_surroundings = getSurroundings(end_coor[0], end_coor[1]);
        int end_type = getSum(end_surroundings);
        GameObject end_GO = Instantiate(end_portal, new Vector3(step * end_coor[0], 1, step * end_coor[1]), findRot(end_type, end_surroundings)) as GameObject;
        start_GO.transform.Translate(Vector3.forward);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject tmp in players)
        {
            tmp.transform.position = new Vector3(step * start_coor[0], -.7f, step * start_coor[1]);
        }

		GameObject torch = GameObject.FindGameObjectWithTag ("Torch");
		torch.transform.position = new Vector3 (step * start_coor [0], -.7f, step * start_coor [1]);

        startPos = new Vector3(step * start_coor[0], -.7f, step * start_coor[1]);

        //Debug.Log("i:"+start_coor[0]+", j:"+start_coor[1]);
    }      
    
    int[] pickRandomCoor()
    {
        while (true)
        {
            int random = Random.Range(0, mazeSize[0]*mazeSize[1]);
            int i = (int)System.Math.Floor((double)random / mazeSize[0]);
            int j = random % mazeSize[0];
            if (getSum(getSurroundings(i, j)) == 2 && maze[i, j])
            {
                return new int[2] { i, j };
            }
        }
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
			GameObject temp = Instantiate(item, pos, rot) as GameObject;
			if (temp.CompareTag ("Torch")) {
				GameObject.Find ("Game Manager").GetComponent<GameManager> ().torchObject = temp;
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
