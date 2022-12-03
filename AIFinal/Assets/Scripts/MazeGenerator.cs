using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]int startSize; //width and height for map at the start
    int proximity; //how many rows the player needs to be from an edge to generate a new row 


    EllerMaze maze;



    // Start is called before the first frame update
    void Start()
    {
        maze = new EllerMaze(startSize);
        maze.BuildMaze();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
