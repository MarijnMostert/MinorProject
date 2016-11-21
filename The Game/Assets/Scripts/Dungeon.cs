using UnityEngine;
using System.Threading;
using System.Collections;

public class Dungeon : MonoBehaviour {
    public GameObject floor, side, sideAlt1, sideAlt2, corner, cornerout,
                            roof, block, player, trap_straight, trap_crossing, 
                            trap_box, portal, cam, ui, pointer;
    public GameObject[,] dungeon;
    int[] mazeSize;
    bool[,] maze, import_maze, trapped;
    float chance_trap_straight, chance_trap_crossing;
    float chance_side_alt1, chance_side_alt2;
    bool start_defined;
    int[] count = new int[2] {0,2};

    // Use this for initialization
    void Start()
    {
        mazeSize = new int[] { 5, 5 };
        chance_trap_straight = 1f;
        chance_trap_crossing = 1f;
        chance_side_alt1 = 0.2f;
        chance_side_alt2 = 0.2f + chance_side_alt1;
        start_defined = false;

        //import_maze = new bool[mazeSize[0], mazeSize[1]];

        //simulate mazecreation
        import_maze = new bool[5, 5] {  {false,false,true,false,false},
                                        {false,false,true,false,false},
                                        {false,false,true,false,false},
                                        {false,false,true,false,false},
                                        {false,false,true,false,false} };
        printMaze(import_maze);

        updateProgress(0.2f);
        mazeSize = new int[] { mazeSize[0] * 3, mazeSize[1] * 3 };
        dungeon = new GameObject[mazeSize[0], mazeSize[1]];
        maze = new bool[mazeSize[0], mazeSize[1]];
        maze = StretchMatrix(import_maze);
        printMaze(maze);
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
                            dungeon[i, j] = Instantiate(chooseFloor(i,j), new Vector3(2f * i, 0, 2f * j), findRotFloor(i,j)) as GameObject;
                            break;
                        case 1:
                            dungeon[i, j] = Instantiate(chooseSide(), new Vector3(2f * i, 0, 2f * j), findRot(type, surroundings)) as GameObject;
                            break;
                        case 2:
                            dungeon[i, j] = Instantiate(corner, new Vector3(2f * i, 0, 2f * j), findRot(type, surroundings)) as GameObject;
                            break;
                        case 3:
                            dungeon[i, j] = Instantiate(cornerout, new Vector3(2f * i, 0, 2f * j), findRot(type, surroundings)) as GameObject;
                            break;
                        default:
                            break;
                    }
                    updateProgress(deltaprogress);
                }
                else
                {
                    dungeon[i, j] = Instantiate(roof, new Vector3(2f * i, 0, 2f * j), Quaternion.Euler(new Vector3(-90,0,0))) as GameObject;
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
            else if (sum == 2 && diagsum == 4)
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

    void printMaze(bool[,] maze)
    {
        string logstring = "";
        for (int i = 0; i < mazeSize[0]; i++)
        {
            for (int j = 0; j < mazeSize[1]; j++)
            {
                if (maze[i, j])
                {
                    logstring += "1 ";
                }
                else
                {
                    logstring += "0 ";
                }
            }
            logstring += "\n";
        }
        Debug.Log(logstring);
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

        GameObject start_GO = Instantiate(portal, new Vector3(2f * start_coor[0], 0, 2f * start_coor[1]), findRot(type, surroundings)) as GameObject;
        start_GO.transform.Translate(Vector3.forward);
     
        Debug.Log("i:"+start_coor[0]+", j:"+start_coor[1]);
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


    // Update is called once per frame
    void Update () {
	}
}
