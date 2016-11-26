using UnityEngine;
using System.Threading;
using System.Collections;

public class DungeonInstantiate : Object {
    GameObject floor, side, sideAlt1, sideAlt2, corner, cornerout,
                            roof, block, trap_straight, trap_crossing, trap_box, 
                            portal, cam, ui, pointer, starters_pack, scene_manager, player;
    GameObject[,] dungeon;
    GameObject[] players;
    int[] mazeSize;
    bool[,] maze, import_maze, trapped;
    float chance_trap_straight, chance_trap_crossing;
    float chance_side_alt1, chance_side_alt2, step;
    bool start_defined;
    int[] count = new int[2] {0,2};

    // Use this for initialization
    public DungeonInstantiate(GameObject floor, GameObject side, GameObject sideAlt1, GameObject sideAlt2, GameObject corner, GameObject cornerout,
                            GameObject roof, GameObject block, GameObject trap_straight, GameObject trap_crossing, GameObject trap_box,
                            GameObject portal, GameObject cam, GameObject ui, GameObject pointer, GameObject starters_pack, GameObject scene_manager,
                            GameObject player, int[] mazeSize)
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
        this.cam = cam;
        this.ui = ui;
        this.pointer = pointer;
        this.starters_pack = starters_pack;
        this.scene_manager = scene_manager;
        this.player = player;
        this.mazeSize = new int[2] { mazeSize[0] - 2, mazeSize[1] - 2 };

    }

    public void createMaze(){
        chance_trap_straight = 1f;
        chance_trap_crossing = 1f;
        chance_side_alt1 = 0.2f;
        chance_side_alt2 = 0.2f + chance_side_alt1;
        start_defined = false;
        step = 2f;

        //import starters pack
        Instantiate(starters_pack, new Vector3(0, 0, 0),Quaternion.identity);
        //Instantiate(scene_manager, new Vector3(0, 0, 0), Quaternion.identity);

        GameObject.Find("Spawner").GetComponent<Spawner>().mapMinX = 5;
        GameObject.Find("Spawner").GetComponent<Spawner>().mapMinZ = 5;
        GameObject.Find("Spawner").GetComponent<Spawner>().mapMaxX = (mazeSize[0]-1)*2*3+5;
        GameObject.Find("Spawner").GetComponent<Spawner>().mapMaxZ = (mazeSize[1] - 1) * 2 * 3 + 5;

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
    }

    void populateMaze()
    {
        float deltaprogress = 0.5f / (mazeSize[0] * mazeSize[1]);
        for (int i = 0; i < mazeSize[0]; i++)
        {
            for (int j = 0; j < mazeSize[1]; j++)
            {
                if (maze[i, j])
                {
                    int[] surroundings = getSurroundings(i, j);
                    int type = getSum(surroundings);
                    switch (type)
                    {
                        case 0:
                            dungeon[i, j] = Instantiate(chooseFloor(i,j), new Vector3(step * i, 0, step * j), findRotFloor(i,j)) as GameObject;
                            break;
                        case 1:
                            dungeon[i, j] = Instantiate(chooseSide(), new Vector3(step * i, 0, step * j), findRot(type, surroundings)) as GameObject;
                            break;
                        case 2:
                            dungeon[i, j] = Instantiate(corner, new Vector3(step * i, 0, step * j), findRot(type, surroundings)) as GameObject;
                            break;
                        case 3:
                            dungeon[i, j] = Instantiate(cornerout, new Vector3(step * i, 0, step * j), findRot(type, surroundings)) as GameObject;
                            break;
                        default:
                            break;
                    }
                    updateProgress(deltaprogress);
                }
                else
                {
                    dungeon[i, j] = Instantiate(roof, new Vector3(step * i, 0, step * j), Quaternion.Euler(new Vector3(-90,0,0))) as GameObject;
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
                if (count[0]==count[1]) {
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

        GameObject start_GO = Instantiate(portal, new Vector3(step * start_coor[0], 0, step * start_coor[1]), findRot(type, surroundings)) as GameObject;
        start_GO.transform.Translate(Vector3.forward);

        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            player.transform.position = new Vector3(step * start_coor[0], -.7f, step * start_coor[1]);
        }

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
