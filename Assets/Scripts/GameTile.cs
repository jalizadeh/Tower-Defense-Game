using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{

    [SerializeField]
    Transform arrow = default;

    //neighbor tiles
    GameTile north, east, south, west;



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
