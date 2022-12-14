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

    bool playerFound = false;

    [SerializeField] float moveSpeed;

    [SerializeField] GameObject cell;

    //Has a reference to the maze generator to access the cells
    [SerializeField] MazeGenerator maze;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Gets where the player currently is
        playerNode = new AStarNode((int)(player.transform.position.x), (int)(player.transform.position.z),
            maze.maze.GetCell((int)(player.transform.position.x), (int)(player.transform.position.z)));


        //gets the AI node by transforming its x and z to grid space
        AStarNode parent = new AStarNode((int)(transform.position.x), (int)(transform.position.z),
            maze.maze.GetCell((int)(transform.position.x), (int)(transform.position.z)));

        Debug.Log("x of node: " + (int)(player.transform.position.x / cell.transform.localScale.x));
        Debug.Log("z of node: " + (int)(player.transform.position.z / cell.transform.localScale.z));

       if (parent != null)
       {
            AStar(parent);
       }
    }

    private void FixedUpdate()
    {
        //MoveToPoint();
    }

    void AStar(AStarNode parent)
    {
        if(playerFound)
        {
            return;
        }

        if(parent == playerNode)
        {
            Debug.Log("Player Found");
            MoveToPoint();
            playerFound = true;
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
        if(player)
        {
            Vector3 pointDirection = (currentPath[1].node.transform.position - transform.position).normalized;

            rb.velocity = (pointDirection * moveSpeed);
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
