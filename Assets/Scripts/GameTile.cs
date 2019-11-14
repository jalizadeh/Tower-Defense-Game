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

    int distance;




    public void ClearPath()
    {
        distance = int.MaxValue;
        nextOnPath = null;
    }


    public void BecomeDestination()
    {
        distance = 0;
        nextOnPath = null;
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


    //For keeping track of path, after checking neighbors,
    // either it returns the neighbor or null
    GameTile GrowPathTo(GameTile neighbor)
    {
        Debug.Assert(HasPath, "No Path");
        if(neighbor == null || neighbor.HasPath)
        {
            return null;
        }
        neighbor.distance = distance + 1;
        neighbor.nextOnPath = this;
        return neighbor;
    }


    public GameTile GrowPathToNorth() => GrowPathTo(north);

    public GameTile GrowPathToSouth() => GrowPathTo(south);

    public GameTile GrowPathToEast() => GrowPathTo(east);

    public GameTile GrowPathToWest() => GrowPathTo(west);


    public static void MakeEastWestNeighbors(GameTile east, GameTile west)
    {
        //if the condition is false, it prints the message
        Debug.Assert(east.west == null && west.east == null, "Redefined neighbors");

        west.east = west;
        east.west = east;
    }

    public static void MakeNorthSouthNeighbors(GameTile north, GameTile south)
    {
        //if the condition is false, it prints the message
        Debug.Assert(north.south == null && south.north == null, "Redefined neighbors");

        south.north = north;
        north.south = south;
    }
}
