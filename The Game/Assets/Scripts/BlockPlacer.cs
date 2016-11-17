using UnityEngine;
using System.Collections;

public class BlockPlacer : MonoBehaviour {
    public GameObject floor,side,corner,corner2,roof,block,player;
    public GameObject[,] Dungeon;
    int[] mazeSize;
    bool[,] maze, import_maze;

    // Use this for initialization
    void Start()
    {
        mazeSize = new int[]{ 5, 5};
        //import_maze = new bool[mazeSize[0], mazeSize[1]];

        //simulate mazecreation
        import_maze = new bool[5, 5] {  {false,true,false,false,false},
                                        {false,true,true,true,false},
                                        {false,false,false,true,false},
                                        {false,false,true,true,false},
                                        {false,false,true,false,false} };
        printMaze(import_maze);

        mazeSize = new int[] { mazeSize[0] * 3, mazeSize[1] * 3 };
        Dungeon = new GameObject[mazeSize[0], mazeSize[1]];
        maze = new bool[mazeSize[0], mazeSize[1]];
        maze = StretchMatrix(import_maze);
        printMaze(maze);

        for (int i = 0; i < mazeSize[0]; i++) {
            for (int j = 0; j < mazeSize[1]; j++) {
                if(maze[i, j]) {
                    int[] surroundings = getSurroundings(i, j);
                    int type = getType(surroundings);
                    switch (type)
                    {
                        case 0:
                            Dungeon[i, j] = Instantiate(floor, new Vector3(2f * i, 0, 2f * j), findRot(type, surroundings)) as GameObject;
                            break;
                        case 1:
                            Dungeon[i, j] = Instantiate(side, new Vector3(2f * i, 0, 2f * j), findRot(type, surroundings)) as GameObject;
                            break;
                        case 2:
                            Dungeon[i, j] = Instantiate(corner, new Vector3(2f * i, 0, 2f * j), findRot(type, surroundings)) as GameObject;
                            break;
                        case 3:
                            Dungeon[i, j] = Instantiate(corner2, new Vector3(2f * i, 0, 2f * j), findRot(type, surroundings)) as GameObject;
                            break;
                        default:
                            break;
                    }
                } else {
                    Dungeon[i, j] = Instantiate(roof, new Vector3(2f * i, 0, 2f * j), Quaternion.identity) as GameObject;
                }
            }
        }
    }

    Quaternion findRot(int type, int[] surroundings){
        switch (type){
            case 0:
                return Quaternion.identity;
            case 1:
                return findRotSide(surroundings);
            case 2:
                return findRotCorner(surroundings);
            default:
                return findRotCorner2(surroundings);
        } 
    }

    Quaternion findRotSide(int[] surroundings) {

        if(ArrayEquals(surroundings,new int[] {1,0,0,0})){
            return Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else if (ArrayEquals(surroundings,new int[] {0,1,0,0})){
            return Quaternion.Euler(new Vector3(0, 90, 0));
        }
        else if (ArrayEquals(surroundings,new int[] {0,0,1,0})){
            return Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else {
            return Quaternion.Euler(new Vector3(0, -90, 0));
        }
    }

    Quaternion findRotCorner(int[] surroundings){
        if (ArrayEquals(surroundings,new int[] {1,1,0,0})){
            return Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else if (ArrayEquals(surroundings,new int[] {0,1,1,0})){
            return Quaternion.Euler(new Vector3(0, 90, 0));
        }
        else if (ArrayEquals(surroundings,new int[] {0,0,1,1})){
            return Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else {
            return Quaternion.Euler(new Vector3(0, -90, 0));
        }
    }

    Quaternion findRotCorner2(int[] surroundings) {
        if (surroundings[0] == 1) {
            return Quaternion.Euler(new Vector3(0, 90, 0));
        }
        else if (surroundings[1] == 1) {
            return Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else if (surroundings[2] == 1) {
            return Quaternion.Euler(new Vector3(0, -90, 0));
        }
        else if (surroundings[3] == 1) {
            return Quaternion.Euler(new Vector3(0, 0, 0));
        } else {
            return Quaternion.Euler(new Vector3(0, 0, 90));
        }
    }

    int[] getSurroundings(int x, int z) {
        int[] surroundings= new int[4];
        surroundings[0] = getMazeValue(x+1, z);
        surroundings[1] = getMazeValue(x, z+1);
        surroundings[2] = getMazeValue(x-1, z);
        surroundings[3] = getMazeValue(x, z-1);
        //Debug.Log("x=" + x + ", z=" + z + ", Surr= [" + surroundings[0]+", "+surroundings[1]+", "+surroundings[2]+", "+surroundings[3]+"]");
        
        return surroundings;
    }

    int getType(int[] surroundings){
        int sum = 0;
        foreach (int tmp in surroundings){
            sum += tmp;
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


	// Update is called once per frame
	void Update () {
	
	}
}
