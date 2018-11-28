using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float moveTime = 0.1f;         //Time it takes to move the player
    public LayerMask blockingLayer;       //Layer used for collision detection
    public bool allowHumanMovement = true; //Set this to false when we implement AI movement

    private BoxCollider2D boxCollider;    //Necesary to collide with other objects, need to disable when raytracing
    private Rigidbody2D rb2D;            //Used to move our object
    private float inverseMoveTime;
    private bool isMoving;                //Prevents input while character is moving
    
    //Initalize game object data
	void Start () {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
        isMoving = false;
    }

    //This function occures on every game tick currently it allows a human to control the player
    private void Update()
    {
        //This runs when the human is controlling the player
        if (allowHumanMovement)
        {
            //Initalize the character's move direction
            int horizontal = 0;
            int vertical = 0;

            //Get the character input
            horizontal = (int)(Input.GetAxisRaw("Horizontal"));
            vertical = (int)(Input.GetAxisRaw("Vertical"));

            //Check if moving horizontally, if so set vertical to zero.
            if (horizontal != 0)
            {
                vertical = 0;
            }

            if (horizontal != 0 || vertical != 0)
            {
                AttemptMove(horizontal, vertical);
            }
        }
    }

    //This function is called by CanMoveUp/Right/Down/Left.
    //It checks if the player can move in the specified direction.
    private bool CanMove (float xDir, float yDir)
    {
        //This waits for the player movement to finish the timeout is just in case something goes wrong to avoid an infinite loop.
        int timeOut = 0;
        while (isMoving)
        {
            timeOut++;
            if (timeOut > 100000)
            {
                Debug.LogError("Error can move timeout");
                return false;
            }
        }

        //Now check if the play can move up.
        RaycastHit2D hit;
        boxCollider.enabled = false;
        Vector2 end = new Vector2(transform.position.x + 1, transform.position.y);
        hit = Physics2D.Linecast(transform.position, end, blockingLayer);
        boxCollider.enabled = true;
        if (hit.transform == null)
            return true;

        return false;
    }

    //Used externally
    public bool CanMoveUp ()
    {
        return CanMove(0, 1);
    }

    //Used externally
    public bool CanMoveRight()
    {
        return CanMove(1, 0);
    }

    //Used externally
    public bool CanMoveDown()
    {
        return CanMove(0, -1);
    }

    //Used externally
    public bool CanMoveLeft()
    {
        return CanMove(-1, 0);
    }

    //This attempts to move the player in the provided direction.
    //Positive X moves right.
    //Positive Y moves up.
    //If the chosen direction is blocked, the player doesn't or can't move and the function returns false.
    //If the chosen direction is vacant, the player moves and the function returns true.
    public bool AttemptMove(int xDir, int yDir)
    {
        RaycastHit2D hit;
        Move(xDir, yDir, out hit);
        //Checks if our ray hit a wall or not.
        if (hit.transform == null)
            return true;
        
        return false;
    }

    //Checkes if the player can move in the given direction.
    //If they can move, return true and start the SmoothMovement coroutine.
    //If they can't move, return false
    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        //Calculate start and end positions for our movement
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        //Check if we will hit a wall
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        //If we are already moving, return false
        if (!isMoving)
        {
            //If we don't hit a wall, begin moving
            if (hit.transform == null)
            {
                StartCoroutine(SmoothMovement(end));
                return true;
            }
        }
        return false;
    }
    
    //This allows the character to appear with smoth movement, we are incramentally setting the character's position to a location closer to the end position
    //This is only called by Move
    private IEnumerator SmoothMovement(Vector3 end)
    {
        isMoving = true;
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        
        while (sqrRemainingDistance > float.Epsilon)
        {
            //Sets the characer's new position
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPostion);

            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
        isMoving = false;
    }
}
