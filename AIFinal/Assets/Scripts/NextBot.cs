using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NextBot : MonoBehaviour
{

    private Rigidbody rb;

    [SerializeField] GameObject player;

    AStarNode playerNode;

    List<AStarNode> currentPath;
    Queue<AStarNode> currentPathQueue;

    bool playerFound = false;

    [SerializeField] float moveSpeed;

    [SerializeField] GameObject cell;

    //Has a reference to the maze generator to access the cells
    [SerializeField] MazeGenerator maze;

    // Start is called before the first frame update
    void Start()
    {
        currentPath= new List<AStarNode>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (maze.generated == true)
        {
            //Gets where the player currently is
            playerNode = new AStarNode((int)(player.transform.position.x), (int)(player.transform.position.z),
                maze.maze.GetCellFromWorld((int)(player.transform.position.x), (int)(player.transform.position.z)));


            //gets the AI node by transforming its x and z to grid space
            AStarNode parent = new AStarNode((int)(transform.position.x), (int)(transform.position.z),
                maze.maze.GetCellFromWorld((int)(transform.position.x), (int)(transform.position.z)));

            Debug.Log("x of node: " + (int)(player.transform.position.x));
            Debug.Log("z of node: " + (int)(player.transform.position.z));

            parent.parent = parent;

            if (parent != null)
            {
                AStar(parent);
            }
        }
    }

    private void FixedUpdate()
    {
        //FacePlayer();
        MoveToPoint();
    }

    void AStar(AStarNode parent)
    {
        if(playerFound)
        {
            return;
        }

        if(parent.node.worldX == playerNode.node.worldX && parent.node.worldZ == playerNode.node.worldZ)
        {
            Debug.Log("Player Found");
            for(int i = 0; i <= currentPath.Count; i++)
            {
                currentPathQueue.Enqueue(currentPath[i]);
            }
            playerFound = true;
            //MoveToPoint();
        }
        else
        {
            
            //parent.print();
            currentPath.Add(parent);
            List<AStarNode> visitables = maze.maze.getVisitables(parent);
            if(visitables.Count == 0) 
            {
                clearToIntersection();
                Debug.Log("Dead end/intersection reached");
                return;
            }

            for (int i = 0; i < visitables.Count; i++)
            {
                AStar(visitables[i]);
            }
        }
    }

    void FacePlayer()
    {
        if(player)
        {
            Vector3 playerDirection = (player.transform.position - transform.position).normalized;

            float face = Mathf.Atan2(playerDirection.z, playerDirection.z) * Mathf.Rad2Deg;

            transform.Rotate(Vector3.forward, face);
        }
    }

    void MoveToPoint()
    {
        if (playerFound)
        {
            if (currentPathQueue.Count > 0)
            {
                Vector3 pointDirection = (currentPathQueue.Peek().node.transform.position - transform.position).normalized;

                transform.position = currentPathQueue.Peek().node.transform.position;

                //transform.position += (pointDirection * moveSpeed);
                

                currentPathQueue.Dequeue();
            }
            else
            {
                currentPath.Clear();
                currentPath = new List<AStarNode>();
                playerFound = false;
            }
        }
    }

    void clearToIntersection()
    {
        for(int i = currentPath.Count - 1; i >= 0; i--)
        {
            if (maze.maze.getVisitables(currentPath[i]).Count == 0 || maze.maze.getVisitables(currentPath[i]).Count == 1)
            {
                currentPath.Remove(currentPath[i]);
            }
            else
            {
                return;
            }
        }
    }
}
