using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    bool north, south, east, west; //walls
    int set; //which set the cell belongs to

    [SerializeField] GameObject northWall, eastWall, southWall, westWall;


}