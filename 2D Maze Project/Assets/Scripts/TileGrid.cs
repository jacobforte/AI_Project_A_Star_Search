using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public LayerMask blockingLayer;         //Layer used for collision detection
    public LayerMask tileLayer;

    private BoxCollider2D boxCollider2D;


    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        isVisited = false;
        //distanceFromStart;

        //Get the distance from the end.
        Vector2 exit = GameObject.FindGameObjectWithTag("Exit").transform.position;

        distanceFromEnd = Mathf.Sqrt(Mathf.Pow((transform.position.x - exit.x), 2f)
                         + Mathf.Pow((transform.position.y - exit.y), 2f));

        //Get the adjacent tiles not blocked by walls.
        RaycastHit2D wallHit, tileHit;
        boxCollider2D.enabled = false;
        //Get the above tile
        Vector2 end = new Vector2(transform.position.x, transform.position.y + 1);
        wallHit = Physics2D.Linecast(transform.position, end, blockingLayer);
        tileHit = Physics2D.Linecast(transform.position, end, tileLayer);
        if (wallHit.collider == null)
        {
            tileUp = tileHit.collider.gameObject.GetComponent<TileGrid>();
        }
        //Get the right tile
        end = new Vector2(transform.position.x + 1, transform.position.y);
        wallHit = Physics2D.Linecast(transform.position, end, blockingLayer);
        tileHit = Physics2D.Linecast(transform.position, end, tileLayer);
        if (wallHit.collider == null)
        {
            tileRight = tileHit.collider.gameObject.GetComponent<TileGrid>();
        }
        //Get the down tile
        end = new Vector2(transform.position.x, transform.position.y - 1);
        wallHit = Physics2D.Linecast(transform.position, end, blockingLayer);
        tileHit = Physics2D.Linecast(transform.position, end, tileLayer);
        if (wallHit.collider == null)
        {
            tileDown = tileHit.collider.gameObject.GetComponent<TileGrid>();
        }
        //Get the left tile
        end = new Vector2(transform.position.x - 1, transform.position.y);
        wallHit = Physics2D.Linecast(transform.position, end, blockingLayer);
        tileHit = Physics2D.Linecast(transform.position, end, tileLayer);
        if (wallHit.collider == null)
        {
            tileLeft = tileHit.collider.gameObject.GetComponent<TileGrid>();
        }
        boxCollider2D.enabled = true;
    }



    private TileGrid tileUp;
    public TileGrid TileUp
    {
        get
        {
            return tileUp;
        }
    }

    private TileGrid tileRight;
    public TileGrid TileRight
    {
        get
        {
            return tileRight;
        }
    }

    private TileGrid tileDown;
    public TileGrid TileDown
    {
        get
        {
            return tileDown;
        }
    }

    private TileGrid tileLeft;
    public TileGrid TileLeft
    {
        get
        {
            return tileLeft;
        }
    }

    public Sprite ColorTile
    {
        get
        {
            return GetComponent<SpriteRenderer>().sprite;
        }
        set
        {
            GetComponent<SpriteRenderer>().sprite = value;
        }
    }

    //Used to mark as visited
    private bool isVisited;
    public bool IsVisited
    {
        get
        {
            return isVisited;
        }
        set
        {
            isVisited = value;
        }
    }

    //Used to track number of tiles from the start
    private float distanceFromStart;
    public float DistanceFromStart
    {
        get
        {
            return distanceFromStart;
        }
        set
        {
            distanceFromStart = value;
        }
    }

    //Used to track straight line distance to end
    private float distanceFromEnd;
    public float DistanceFromEnd
    {
        get
        {
            return distanceFromEnd;
        }
    }

    //Used when backtracking to highling the best path at the end.
    private TileGrid backtrackTile;
    public TileGrid BacktrackTile
    {
        get
        {
            return backtrackTile;
        }
        set
        {
            backtrackTile = value;
        }
    }
}