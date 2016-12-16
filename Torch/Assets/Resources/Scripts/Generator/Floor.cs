using UnityEngine;
using System.Collections;

public class Floor : Object {
    float MaxZ, MaxX, MinZ, MinX;
    float x, z;

	public Floor (float x, float z)
    {
        this.MaxX = x + 1f;
        this.MaxZ = z + 1f;
        this.MinX = x - 1f;
        this.MinZ = z - 1f;
        this.x = x;
        this.z = z;
    }

    public bool checkFloor(float x, float z)
    {
        return true;
        //return (MinX < x && x < MaxX) && (MinZ < z && z < MaxZ);
    }

    public bool equals(float x, float z)
    {
        return this.x == x && this.z == z;
    }

    public string print()
    {
        return "floor: " + x + ", " + z + " | extra: X = " + MinX + ", " + MaxX + ", Z = " + MinZ + ", " + MaxZ;
    }
}
