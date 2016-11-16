using UnityEngine;
using System.Collections;

public class InstantiatePath : MonoBehaviour {
    public GameObject floor,side,corner,roof,block,player;
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
                int surroundings = getAllSurroundings(i, j);
                if (surroundings == 0){
                    Dungeon[i, j] = Instantiate(floor,new Vector3(10f * i, 0, 10f * j),Quaternion.identity) as GameObject;
                } else if (surroundings == 1){
                    Dungeon[i, j] = Instantiate(side, new Vector3(10f * i, 0, 10f * j), Quaternion.identity) as GameObject;
                }
                else if (surroundings == 2){
                    Dungeon[i, j] = Instantiate(corner, new Vector3(10f * i, 0, 10f * j), Quaternion.identity) as GameObject;
                }
                else {
                    Dungeon[i, j] = Instantiate(roof, new Vector3(10f * i, 0, 10f * j), Quaternion.identity) as GameObject;
                }
            }
        }
    }

    int getAllSurroundings(int x, int z){
        int value = 0;
        value += getMazeValue(x-1, z-1);
        value += getMazeValue(x-1, z+1);
        value += getMazeValue(x+1, z-1);
        value += getMazeValue(x+1, z+1);
        return value;
    }

    int getMazeValue(int x, int z){
        if (inBounds(x, z)){
            return 0;
        }    
        return 1;
    }



    bool inBounds(int x, int z){
        return 0<x && x<mazeSize && 0<z && z<mazeSize;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
