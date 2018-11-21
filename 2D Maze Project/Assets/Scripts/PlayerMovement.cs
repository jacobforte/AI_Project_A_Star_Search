using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float moveTime = 0.1f;   //Time it takes to move the player
    public LayerMask blockingLayer; //This layer checks for collision

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;
    private bool isMoving;
    
	void Start () {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
        isMoving = false;
    }

    private void Update()
    {
        if (!isMoving)
        {
            //Initalize the player's move direction
            int horizontal = 0;
            int vertical = 0;

            //Get the character input
            horizontal = (int)(Input.GetAxisRaw("Horizontal")) * 2;
            vertical = (int)(Input.GetAxisRaw("Vertical")) * 2;

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

    //Return true if the player can move
    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;
        
        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }
    
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        isMoving = true;
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            
            rb2D.MovePosition(newPostion);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
        isMoving = false;
    }

    void AttemptMove(int xDir, int yDir)
    {
        RaycastHit2D hit;
        Move(xDir, yDir, out hit);
        if (hit.transform == null)
            return;
        
        //Wall hitComponent = hit.transform.GetComponent<Wall>();
        //if (!canMove && hitComponent != null)
        //    OnCantMove(hitComponent);
    }

    //void OnCantMove(T component)
    //{
    //
    //}
}
