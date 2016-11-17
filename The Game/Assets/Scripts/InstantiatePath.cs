using UnityEngine;
using System.Collections;

public class InstantiatePath : MonoBehaviour {
    public GameObject floor,side,corner,corner2,roof,block,player;
    public GameObject[,] Dungeon;
    int mazeSize;
    bool[,] maze;

    // Use this for initialization
    void Start()
    {
        mazeSize = 10;
        maze = new bool[mazeSize, mazeSize];
        Dungeon = new GameObject[mazeSize, mazeSize];

        for (int i = 0; i<mazeSize;i++) {
            for (int j = 0; j<mazeSize; j++) {
                maze[i, j] = true;
            }
        }

        for (int i = 0; i < mazeSize; i++){
            for (int j = 0; j < mazeSize; j++){
                int[] surroundings = getSurroundings(i, j);
                int type = getType(surroundings);
                switch (type){
                    case 0:
                        Dungeon[i, j] = Instantiate(floor, new Vector3(2f * i, 0, 2f * j), findRot(type,surroundings)) as GameObject;
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
                        Dungeon[i, j] = Instantiate(roof, new Vector3(2f * i, 0, 2f * j), findRot(type, surroundings)) as GameObject;
                        break;
                }
            }
        }
        for (int i = 0; i < mazeSize; i++) {
            string logstring = "";
            for (int j = 0; j < mazeSize; j++){
                if (maze[i, j]){
                    logstring += "1 ";
                }
                else {
                    logstring += "0 ";
                }
            }
            Debug.Log(logstring);
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
        if (surroundings[0]==1 && surroundings[1]==0){
            return Quaternion.Euler(new Vector3(0,180,0));
        } else if (surroundings[0]==1 && surroundings[1]==1){
            return Quaternion.Euler(new Vector3(0, -90, 0));
        } else if (surroundings[0]==0 && surroundings[1]==1){
            return Quaternion.Euler(new Vector3(0, 90, 0));
        } else {
            return Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }

    Quaternion findRotCorner(int[] surroundings){
        if (surroundings[0] == 1 && surroundings[1] == 1 && surroundings[2] == 1){
            return Quaternion.Euler(new Vector3(0, 90, 0));
        }
        else if (surroundings[0] == 0 && surroundings[1] == 1 && surroundings[2] == 1){
            return Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else if (surroundings[0] == 1 && surroundings[1] == 0 && surroundings[2] == 1){
            return Quaternion.Euler(new Vector3(0, -90, 0));
        }
        else {
            return Quaternion.Euler(new Vector3(0, 0, 0));
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
        Debug.Log("x=" + x + ", z=" + z + ", Surr= [" + surroundings[0]+", "+surroundings[1]+"");
        
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
            return 0;
        }    
        return 1;
    }



    bool inBounds(int x, int z){
        return 0<=x && x<mazeSize && 0<=z && z<mazeSize;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
