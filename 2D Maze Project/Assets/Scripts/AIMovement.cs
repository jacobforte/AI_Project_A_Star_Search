using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour {
    public TileGrid startingTile;           //Holds the data for the starting position. Use startingTile.position.x
    public Sprite YellowTile;               //Used to visually mark a tile, use TileGridObject.ColorTile = YellowTile
    public Sprite RedTile;                  //Used to visually mark a tile, use TileGridObject.ColorTile = RedTile
    public Sprite GreenTile;                //Used to visually mark a tile, use TileGridObject.ColorTile = GreenTile

    private TileGrid currentTile;           //Holds data for the Tile the player is curently on.
    private TileGrid previousTile;          //Holds data for the previous Tile the player was on.
    private TileGrid[] fringeTiles;         //Will hold the data for the fringe Tiles.
    private PlayerMovement playerMovement;  //Access the functions in the player movement script.

	// Use this for initialization
	void Start () {
        playerMovement = GetComponent<PlayerMovement>();

        currentTile = startingTile;
        previousTile = startingTile;
        fringeTiles[0] = startingTile;  //When switching between fringe Tiles, I recomend using PlyayerMovement.ChangePosition for the player.
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
        //currentTile.DistanceFromEnd returns the distance the player curently is from the end.
        //currentTile.TileDown.DistanceFromEnd returns the straight line distance to the end from the tile bellow the player.
        //Use playerMovement.ChangePosition(currentTile.GetComponent<Transform>().position); to move the player directly to a fringe Tile.
        currentTile.ColorTile = YellowTile;

        //If the tile above the player not blocked by a wall AND the tile above is not the previous tile.
        if (currentTile.TileUp != null && currentTile.TileUp != previousTile)
        {
            playerMovement.AttemptMove(0, 1);   //Move the charater up
            previousTile = currentTile;         //Set the previous Tile to teh current Tile
            currentTile = currentTile.TileUp;
            currentTile.ColorTile = YellowTile;
        }
        else if (currentTile.TileRight != null && currentTile.TileRight != previousTile)
        {
            playerMovement.AttemptMove(1, 0);   //Move the charater right
            previousTile = currentTile;         //
            currentTile = currentTile.TileRight;
            currentTile.ColorTile = YellowTile;
        }
        else if (currentTile.TileDown != null && currentTile.TileDown != previousTile)
        {
            playerMovement.AttemptMove(0, -1);   //Move the charater down
            previousTile = currentTile;
            currentTile = currentTile.TileDown;
            currentTile.ColorTile = YellowTile;
        }
        else if (currentTile.TileLeft != null && currentTile.TileLeft != previousTile)
        {
            playerMovement.AttemptMove(-1, 0);   //Move the charater left
            previousTile = currentTile;
            currentTile = currentTile.TileLeft;
            currentTile.ColorTile = YellowTile;
        }
    }
}
