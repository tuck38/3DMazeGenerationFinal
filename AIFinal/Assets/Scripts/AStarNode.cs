using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{

    public AStarNode(int x2, int z2, MazeCell node2)
    {
        x = x2;
        z = z2;
        node = node2;
    }

    public AStarNode(int x2, int z2, MazeCell node2, AStarNode parent2)
    {
        x = x2;
        z = z2;
        node = node2;
        parent = parent2;
    }

    //made ints so the floats round
    int x, z;

    public MazeCell node;
    //parent node, the one that came before it in the maze
    public AStarNode parent = null;
}
