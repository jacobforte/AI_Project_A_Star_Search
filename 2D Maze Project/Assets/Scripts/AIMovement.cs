using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIMovement : MonoBehaviour
{

    public TileGrid startingNode; // setup in unity Holds the data for the starting position.
    public TileGrid endingNode; // setup in unity as the ending tile.

    // use TileGridObject.ColorTile = YellowTile
    public Sprite YellowTile;   //Used to visually mark a visited tile
    public Sprite RedTile;      //Used to visually mark an ending tile
    public Sprite GreenTile;    //Used to visually mark a path tile
    public Sprite PurpleTile;   //Used to visually mark a fringe node tile


    public float updateRate = 0.0f;        // 0.0 Is default value, if you want to use this, 
                                           // use the variable attached to the player.

    private TileGrid currentNode;          // Holds data for the node the player is curently on.
    private TileGrid backtrackNode;        // Holds data for the previous node the player was on.
    private List<TileGrid> fringeNodes;
    private PlayerMovement playerMovement; //Access the functions in the player movement script.


    // Use this for initialization
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        startingNode.DistanceFromStart = 0;

        startingNode.IsVisited = true;
        startingNode.ColorTile = YellowTile;
        currentNode = startingNode;
        fringeNodes = new List<TileGrid>();
        InvokeRepeating("SlowUpdate", 0.0f, updateRate);
    }

    // Slow update is used so us humans can see what is going on change the 
    // updateRate variable on teh player object to change this.
    void SlowUpdate()
    {
        if (!playerMovement.IsMoving)   //Wait untill the player stops moving.
        {
            if (currentNode != endingNode)
            {
                ReallyIncreadiblySmartAIMove();
            }
            // Puzzle Completed!
            else
            {
                backtrackNode = currentNode;
                while (backtrackNode != startingNode)
                {
                    backtrackNode.ColorTile = GreenTile;
                    backtrackNode = backtrackNode.BacktrackTile;
                }
                startingNode.ColorTile = GreenTile;
            }

        }
    }

    // this function should
    // add all neighbors to finge node list.
    // pick the fringe node with the least distance traveled+est distance to the goal.
    // move to that node.
    private void ReallyIncreadiblySmartAIMove()
    {
        // If the tile above the player not blocked by a wall 
        // AND the tile above is not the visited tile. 
        if (currentNode.TileUp != null && !currentNode.TileUp.IsVisited)
        {
            if (!isAFringeNode(currentNode.TileUp))
            {
                // set distance from the start to be 1 + current distance.
                currentNode.TileUp.DistanceFromStart = currentNode.DistanceFromStart + 1;
                // get back track tile to be current spot.
                currentNode.TileUp.BacktrackTile = currentNode;
                // find distance and set it in the tile.
                currentNode.TileUp.AStarDist = findDistance(currentNode.TileUp);
                // color tile purple.
                currentNode.TileUp.ColorTile = PurpleTile;
                // add tile to list of fringe nodes.
                fringeNodes.Add(currentNode.TileUp);
            }

        }
        if (currentNode.TileRight != null && !currentNode.TileRight.IsVisited)
        {
            if (!isAFringeNode(currentNode.TileRight))
            {
                currentNode.TileRight.DistanceFromStart = currentNode.DistanceFromStart + 1;
                currentNode.TileRight.BacktrackTile = currentNode;
                currentNode.TileRight.AStarDist = findDistance(currentNode.TileRight);
                currentNode.TileRight.ColorTile = PurpleTile;

                fringeNodes.Add(currentNode.TileRight);
            }
        }
        if (currentNode.TileDown != null && !currentNode.TileDown.IsVisited)
        {
            if (!isAFringeNode(currentNode.TileDown))
            {
                currentNode.TileDown.DistanceFromStart = currentNode.DistanceFromStart + 1;
                currentNode.TileDown.BacktrackTile = currentNode;
                currentNode.TileDown.AStarDist = findDistance(currentNode.TileDown);
                currentNode.TileDown.ColorTile = PurpleTile;

                fringeNodes.Add(currentNode.TileDown);
            }
        }
        if (currentNode.TileLeft != null && !currentNode.TileLeft.IsVisited)
        {

            if (!isAFringeNode(currentNode.TileLeft))
            {
                currentNode.TileLeft.DistanceFromStart = currentNode.DistanceFromStart + 1;
                currentNode.TileLeft.BacktrackTile = currentNode;
                currentNode.TileLeft.AStarDist = findDistance(currentNode.TileLeft);
                currentNode.TileLeft.ColorTile = PurpleTile;

                fringeNodes.Add(currentNode.TileLeft);
            }
        }

        // print distances and names of fringe nodes for checking.
        for (int i = 0; i < fringeNodes.Count; ++i)
        {
            Debug.Log(("Astar dis = " + fringeNodes[i].AStarDist + " Object Name = " + fringeNodes[i].gameObject.name));
        }
        print("END OF LIST!!");


        // Find the best choice from the fringe nodes
        TileGrid nextNode = fringeNodes[0];
        int index = 0;
        for (int i = 1; i < fringeNodes.Count; ++i)
        {
            if (fringeNodes[i].AStarDist < nextNode.AStarDist)
            {
                nextNode = fringeNodes[i];
                index = i;
            }
        }
        // remove node from list.
        fringeNodes.RemoveAt(index);
        // move the player to the fringe node.
        playerMovement.ChangePosition(nextNode);
        currentNode = nextNode;



        // set color and visit of current tile
        currentNode.ColorTile = YellowTile;
        currentNode.IsVisited = true;

    }

    private float findDistance(TileGrid n)
    {
        // get the distance from the node passed to the finish
        // adds 1 and the distance from the start.
        // returns float
        return n.DistanceFromEnd + 1.0f + n.DistanceFromStart;

    }

    // checks to see if the node is in the array of fringe nodes.
    private bool isAFringeNode(TileGrid n)
    {
        for (int i = 0; i < fringeNodes.Count; ++i)
        {
            if (fringeNodes[i] == n)
                return true;
        }
        return false;
    }
}