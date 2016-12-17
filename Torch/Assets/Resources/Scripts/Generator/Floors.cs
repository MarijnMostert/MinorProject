using UnityEngine;
using System.Collections.Generic;

public class Floors : Object {
    List<Floor> floor_list;

    public Floors()
    {
        floor_list = new List<Floor>();
    }

    public void addfloor(float x, float z)
    {
        if (!contains(x, z))
        {
            floor_list.Add(new Floor(x, z));
        }
    }

    bool contains(float x, float z)
    {
        foreach(Floor floor in floor_list)
        {
            if (floor.equals(x,z))
            {
                return true;
            }
        }
        return false;
    }

    public bool validPosition(float x, float z)
    {
        foreach(Floor floor in floor_list)
        {
            if (floor.checkFloor(x, z))
            {
                return true;
            }
        }
        return false;
    }

    public void importFloorList(bool[,] maze, int[] mazeSize)
    {
        for (int i = 0; i < mazeSize[0]; i++){
            for (int j = 0; j< mazeSize[1]; j++){
                if(maze[i, j])
                {
                    addfloor(i * 6, j * 6);
                }
            }
        }
    }

    public string print()
    {
        string print = "";
        foreach(Floor floor in floor_list)
        {
            print += floor.print() + System.Environment.NewLine;
        }
        return print;
    }
}
