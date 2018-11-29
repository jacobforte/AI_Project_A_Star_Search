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

        //Get the distance from the end.
        Vector2 exit = GameObject.FindGameObjectWithTag("Exit").transform.position;
        distanceFromEnd = Mathf.Sqrt(Mathf.Abs(transform.position.x - exit.x) + Mathf.Abs(transform.position.y - exit.y));

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

    private float distanceFromEnd;
    public float DistanceFromEnd
    {
        get
        {
            return distanceFromEnd;
        }
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
}