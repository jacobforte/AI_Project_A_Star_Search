using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour {
    public TileGrid startingNode;           //Holds the data for the starting position. Use startingNode.position.x
    public Sprite YellowTile;               //Used to visually mark a tile, use TileGridObject.ColorTile = YellowTile
    public Sprite RedTile;                  //Used to visually mark a tile, use TileGridObject.ColorTile = RedTile

    private TileGrid currentNode;           //Holds data for the node the player is curently on.
    private TileGrid previousNode;          //Holds data for the previous node the player was on.
    private TileGrid[] fringeNodes;         //Will hold the data for the fringe nodes.
    private PlayerMovement playerMovement;  //Access the functions in the player movement script.

	// Use this for initialization
	void Start () {
        playerMovement = GetComponent<PlayerMovement>();

        currentNode = startingNode;
        previousNode = startingNode;
        fringeNodes[0] = startingNode;  //When switching between fringe nodes, I recomend using PlyayerMovement.ChangePosition for the player.
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!playerMovement.IsMoving)   //Wait untill the player stops moving. This is important don't remove.
        {
            DummyAIMove();
        }
    }

    private void DummyAIMove ()
    {
        //TileUp/Right/Down/Left return an instance of the tile grid class.
        //currentNode.DistanceFromEnd returns the distance the player curently is from the end.
        //currentNode.TileDown.DistanceFromEnd returns the straight line distance to the end from the tile bellow the player.
        //Use playerMovement.ChangePosition(currentNode.GetComponent<Transform>().position); to move the player directly to a fringe node.
        currentNode.ColorTile = YellowTile;

        //If the tile above the player not blocked by a wall AND the tile above is not the previous tile.
        if (currentNode.TileUp != null && currentNode.TileUp != previousNode)
        {
            playerMovement.AttemptMove(0, 1);   //Move the charater up
            previousNode = currentNode;         //Set the previous node to teh current node
            currentNode = currentNode.TileUp;
            currentNode.ColorTile = YellowTile;
        }
        else if (currentNode.TileRight != null && currentNode.TileRight != previousNode)
        {
            playerMovement.AttemptMove(1, 0);   //Move the charater right
            previousNode = currentNode;         //
            currentNode = currentNode.TileRight;
            currentNode.ColorTile = YellowTile;
        }
        else if (currentNode.TileDown != null && currentNode.TileDown != previousNode)
        {
            playerMovement.AttemptMove(0, -1);   //Move the charater down
            previousNode = currentNode;
            currentNode = currentNode.TileDown;
            currentNode.ColorTile = YellowTile;
        }
        else if (currentNode.TileLeft != null && currentNode.TileLeft != previousNode)
        {
            playerMovement.AttemptMove(-1, 0);   //Move the charater left
            previousNode = currentNode;
            currentNode = currentNode.TileLeft;
            currentNode.ColorTile = YellowTile;
        }
    }
}
