using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    int set; //which set the cell belongs to

    public int x, z;
    public int worldX, worldZ;

    [SerializeField] GameObject northWall, eastWall, southWall, westWall;

    bool north = true;
    bool south = true;
    bool east = true;
    bool west = true;
        

    public void SetNorth(bool value)
    {
        north = value;
        if (northWall != null) northWall.SetActive(value);
    }

    public void SetSouth(bool value)
    {
        south = value;
        if (southWall != null) southWall.SetActive(value);
    }

    public void SetEast(bool value)
    {
        east = value;
        if (eastWall != null) eastWall.SetActive(value);
    }

    public void SetWest(bool value)
    {
        west = value;
        if (westWall != null) westWall.SetActive(value);
    }

    public bool GetNorth() { return north; }
    public bool GetSouth() { return south; }
    public bool GetEast() { return east; }
    public bool GetWest() { return west; }

    public void SetSet(int value) { set = value; }
    public int GetSet() { return set; }

    
    public void CopyCell(MazeCell toCopy)
    {
        this.SetNorth(toCopy.GetNorth());
        this.SetSouth(toCopy.GetSouth());
        this.SetEast(toCopy.GetEast());
        this.SetWest(toCopy.GetWest());
        this.SetSet(toCopy.GetSet());
    }

    public string print()
    {
        string result = "cell X: " + x + ", cell Z: " + z + ", N E S W: " + (north ? 1 : 0) + " " + (east ? 1 : 0) + " " + (south ? 1 : 0) + " " + (west ? 1 : 0);
        return result;
    }

}