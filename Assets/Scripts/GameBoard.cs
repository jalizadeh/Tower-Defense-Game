using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField]
    Transform ground = default;

    [SerializeField]
    GameTile tilePrefab = default;
    GameTile[] tiles;

    Vector2Int size;

    //we need to access the same order as tiles are inserted
    Queue<GameTile> searchFrontier = new Queue<GameTile>();


    public void Initialize(Vector2Int size)
    {
        this.size = size;
        ground.localScale = new Vector3(size.x, size.y, 1f);
        tiles = new GameTile[size.x * size.y];

        int i = 0;
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                
                GameTile tile = tiles[i] = Instantiate(tilePrefab,
                    new Vector3(x - (size.x - 1) * 0.5f, 0f, y - (size.y - 1) * 0.5f),
                    Quaternion.identity);
                tile.name = "Tile [" + x + "," + y + "] / " + i;
                tile.transform.SetParent(transform, false);

                if(y > 0)
                {
                    GameTile.MakeNorthSouthNeighbors(tile, tiles[i - 1]);
                    //Debug.Log("Tile [" + x + "," + y + "] is NS to " + (i - 1));
                }

                if(x > 0)
                {
                    GameTile.MakeEastWestNeighbors(tile, tiles[i - size.x]);
                    //Debug.Log("Tile [" + x + "," + y + "] is EW to " + (i - size.x));
                }

                i++;
            }
        }


        FindPaths();
    }


    void FindPaths()
    {
        foreach (GameTile gt in tiles)
        {
            gt.ClearPath();
        }

        tiles[0].BecomeDestination();
        searchFrontier.Enqueue(tiles[0]);

        while (searchFrontier.Count > 0)
        {
            GameTile tile = searchFrontier.Dequeue();
            if (tile != null)
            {
                searchFrontier.Enqueue(tile.GrowPathToNorth());
                searchFrontier.Enqueue(tile.GrowPathToEast());
                searchFrontier.Enqueue(tile.GrowPathToSouth());
                searchFrontier.Enqueue(tile.GrowPathToWest());
            }
        }


    
}
