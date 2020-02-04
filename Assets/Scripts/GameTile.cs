using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{

    [SerializeField]
    Transform arrow = default;

    //neighbor tiles
    GameTile north, east, south, west;

    GameTile nextOnPath;
    public GameTile NextTileOnPath => nextOnPath;

    public Vector3 ExitPoint
    {
        get;
        private set;
    }

    int distance;

    static Quaternion
            northRotation = Quaternion.Euler(90f, 0f, 0f),
            eastRotation = Quaternion.Euler(90f, 90f, 0f),
            southRotation = Quaternion.Euler(90f, 180f, 0f),
            westRotation = Quaternion.Euler(90f, 270f, 0f);


    public bool IsAlternative { get; set; }

    GameTileContent content;
    public GameTileContent Content
    {
        get => content;

        set
        {
            Debug.Assert(value != null, "Null assigned content");

            if(content != null)
            {
                content.Recycle();
            }

            content = value;
            content.transform.localPosition = transform.localPosition;
        }
    }


    /*
     * equal to:
     * public bool HasPath {
     *      get {
     *         return distance != int.MaxValue;
     *         }
     * }
     */
    public bool HasPath => distance != int.MaxValue;

    public Direction PathDirection
    {
        get;
        private set;
    }


    //------------------------------------------------------------

    public void ClearPath()
    {
        distance = int.MaxValue;
        nextOnPath = null;
    }


    public void BecomeDestination()
    {
        distance = 0;
        nextOnPath = null;
        ExitPoint = transform.localPosition;
    }


    //For keeping track of path, after checking neighbors,
    // either it returns the neighbor or null
    //It starts from destination and surfs through neighbors
    GameTile GrowPathTo(GameTile neighbor, Direction direction)
    {
        Debug.Assert(HasPath, "No Path");
        if(neighbor == null || neighbor.HasPath || !HasPath)
        {
            return null;
        }

        neighbor.distance = distance + 1;
        neighbor.nextOnPath = this;
        neighbor.PathDirection = direction;
        neighbor.ExitPoint = (transform.localPosition + neighbor.transform.localPosition) * 0.5f;

        //if the neighbor is not a wall, return it
        return neighbor.Content.BlockPath ? null : neighbor;
    }


    //The directions are opposite, which means:
    // `this tile is located X of the neighbor`
    public GameTile GrowPathToNorth() => GrowPathTo(north, Direction.South);

    public GameTile GrowPathToSouth() => GrowPathTo(south, Direction.North);

    public GameTile GrowPathToEast() => GrowPathTo(east, Direction.West);

    public GameTile GrowPathToWest() => GrowPathTo(west, Direction.East);


    public void ShowPath()
    {
        if(distance == 0)
        {
            arrow.gameObject.SetActive(false);
            return;
        }

        arrow.gameObject.SetActive(true);

        //while initializing, I saved all neighbors, so now I can compare
        // with them, to find which one is the path
        arrow.localRotation =
            nextOnPath == north ? northRotation :
            nextOnPath == east ? eastRotation :
            nextOnPath == west ? westRotation :
            southRotation;
    }

    public void HidePath()
    {
        arrow.gameObject.SetActive(false);
    }

    public static void MakeEastWestNeighbors(GameTile east, GameTile west)
    {
        //if the condition is false, it prints the message
        Debug.Assert(east.west == null && west.east == null, "Redefined neighbors");

        west.east = east;
        east.west = west;
    }

    public static void MakeNorthSouthNeighbors(GameTile north, GameTile south)
    {
        //if the condition is false, it prints the message
        Debug.Assert(north.south == null && south.north == null, "Redefined neighbors");

        south.north = north;
        north.south = south;
    }
}
