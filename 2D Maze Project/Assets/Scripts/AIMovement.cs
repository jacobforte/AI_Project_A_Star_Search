using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    // setup in unity Holds the data for the starting position.
    public TileGrid startingNode;
    // setup in unity as the ending tile.
    public TileGrid endingNode;
    public Sprite YellowTile;   //Used to visually mark a tile, use TileGridObject.ColorTile = YellowTile
    public Sprite RedTile;      //Used to visually mark a tile, use TileGridObject.ColorTile = RedTile
    public Sprite GreenTile;    //Used to visually mark a tile, use TileGridObject.ColorTile = GreenTile
    public Sprite PurpleTile;
    public float updateRate = 0.0f; //0.0 Is default value, if you want to use this, use the variable attached to the player.

    private TileGrid currentNode;           //Holds data for the node the player is curently on.
    private TileGrid backtrackNode;          //Holds data for the previous node the player was on.
    private List<TileGrid> fringeNodes;
    private List<float> nodeDistances;
    private PlayerMovement playerMovement;  //Access the functions in the player movement script.


    // Use this for initialization
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        startingNode.DistanceFromStart = 0;

        currentNode = startingNode;
        fringeNodes = new List<TileGrid>();
        nodeDistances = new List<float>();

        InvokeRepeating("SlowUpdate", 0.0f, updateRate);
    }

    // Slow update is used so us humans can see what is going on change the updateRate variable on teh player object to change this.
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
            // color currentNode to highlight path.
            // current node = backtrackNode.


        }
    }

    // this function should
    // add all neighbors to finge node list.
    // pick the fringe node with the least distance traveled+est distance to the goal.
    // move to that node.
    private void ReallyIncreadiblySmartAIMove()
    {

        // Use playerMovement.ChangePosition(currentNode); 
        // to move the player directly to a fringe node.


        // set color and distance of current tile.
        // color should change to red if visited already.
        currentNode.ColorTile = YellowTile;
        currentNode.IsVisited = true;


        // If the tile above the player not blocked by a wall 
        // AND the tile above is not the visited tile. 
        if (currentNode.TileUp != null && !currentNode.TileUp.IsVisited)
        {
            if (!isAFringeNode(currentNode.TileUp))
            {
                currentNode.TileUp.DistanceFromStart = currentNode.DistanceFromStart + 1;
                currentNode.TileUp.BacktrackTile = currentNode;
                fringeNodes.Add(currentNode.TileUp);

                // set backTracktile to currentNode

                float aStarDist = findDistance(currentNode.TileUp);
                nodeDistances.Add(aStarDist);
            }

        }
        if (currentNode.TileRight != null && !currentNode.TileRight.IsVisited)
        {
            if (!isAFringeNode(currentNode.TileRight))
            {
                currentNode.TileRight.DistanceFromStart = currentNode.DistanceFromStart + 1;

                currentNode.TileRight.BacktrackTile = currentNode;
                fringeNodes.Add(currentNode.TileRight);
                float aStarDist = findDistance(currentNode.TileRight);
                nodeDistances.Add(aStarDist);
            }
        }
        if (currentNode.TileDown != null && !currentNode.TileDown.IsVisited)
        {
            if (!isAFringeNode(currentNode.TileDown))
            {
                currentNode.TileDown.DistanceFromStart = currentNode.DistanceFromStart + 1;

                currentNode.TileDown.BacktrackTile = currentNode;
                fringeNodes.Add(currentNode.TileDown);
                float aStarDist = findDistance(currentNode.TileDown);
                nodeDistances.Add(aStarDist);

            }
        }
        if (currentNode.TileLeft != null && !currentNode.TileLeft.IsVisited)
        {
            currentNode.TileLeft.DistanceFromStart = currentNode.DistanceFromStart + 1;

            currentNode.TileLeft.BacktrackTile = currentNode;
            if (!isAFringeNode(currentNode.TileLeft))
            {
                fringeNodes.Add(currentNode.TileLeft);
                float aStarDist = findDistance(currentNode.TileLeft);
                nodeDistances.Add(aStarDist);
            }
        }

        // print names of tile to make sure they are added correctly
        // foreach(TileGrid Tile in fringeNodes){
        //     Debug.Log(Tile.gameObject.name);
        // }


        for (int i = 0; i < nodeDistances.Count; ++i)
        {
            print(fringeNodes[i].DistanceFromEnd);
            print(fringeNodes[i].gameObject.name);
        }

        // FIND THE BEST CHOICE FROM THE FRINDGE NODES.
        // iterate through the distances list and find the index with the lowest value.
        // 
        int indexOfNextBestNode = 0;
        float bestDistance = nodeDistances[0];
        for (int i = 1; i < nodeDistances.Count; ++i)
        {
            if (nodeDistances[i] < bestDistance)
            {
                indexOfNextBestNode = i;
            }
        }

        // get node from fringenodes
        // remove it from the fringenodes and nodeDistances lists
        TileGrid nextNode = fringeNodes[indexOfNextBestNode];

        // print("Taking node:");
        // print(nextNode);

        fringeNodes.RemoveAt(indexOfNextBestNode);
        nodeDistances.RemoveAt(indexOfNextBestNode);



        // move the player to the fringe node.
        playerMovement.ChangePosition(nextNode);
        currentNode = nextNode;

        if (currentNode == endingNode)
        {
            return;
        }
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
