using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int startSize; //width and height for map at the start
    int proximity; //how many rows the player needs to be from an edge to generate a new row 


    public EllerMaze maze;

    public bool generated = false;

    // Start is called before the first frame update
    void Start()
    {
        maze = new EllerMaze(startSize, startSize);
        maze.BuildMaze();
        generated = true;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKeyDown("space"))
        {
            maze.BuildSouth();
        }
    }
}
