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

    GameTileContentFactory contentFactory;


    public void Initialize(Vector2Int size, GameTileContentFactory contentFactory)
    {
        this.size = size;
        this.contentFactory = contentFactory;
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

                tile.IsAlternative = (x & 1) == 0;
                if((y & 1) == 0)
                {
                    tile.IsAlternative = !tile.IsAlternative;
                }

                //by default all tiles are set to Empty
                tile.Content = contentFactory.Get(GameTileContentType.Empty);

                i++;
            }
        }

        //start with a default destination at the center
        ToggleDestination(tiles[tiles.Length / 2]);
    }


    bool FindPaths()
    {
        foreach (GameTile tile in tiles)
        {
            if(tile.Content.Type == GameTileContentType.Destination)
            {
                tile.BecomeDestination();
                searchFrontier.Enqueue(tile);
            } else
            {
                tile.ClearPath();
            }
        }

        while (searchFrontier.Count > 0)
        {
            GameTile tile = searchFrontier.Dequeue();
            if (tile != null)
            {
                if (tile.IsAlternative)
                {
                    searchFrontier.Enqueue(tile.GrowPathToNorth());
                    searchFrontier.Enqueue(tile.GrowPathToSouth());
                    searchFrontier.Enqueue(tile.GrowPathToEast());
                    searchFrontier.Enqueue(tile.GrowPathToWest());
                }
                else
                {
                    searchFrontier.Enqueue(tile.GrowPathToEast());
                    searchFrontier.Enqueue(tile.GrowPathToWest());
                    searchFrontier.Enqueue(tile.GrowPathToNorth());
                    searchFrontier.Enqueue(tile.GrowPathToSouth());
                }
            }
        }

        
        foreach (GameTile tile in tiles)
        {
            tile.ShowPath();
        }

        if (searchFrontier.Count == 0)
        {
            return false;
        }

        return true;
    }


    //Get the tile based on mouse position
    public GameTile GetTile(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            int x = (int)(hit.point.x + size.x * 0.5f);
            int y = (int)(hit.point.z + size.y * 0.5f);

            if (x >= 0 && x < size.x && y >= 0 && y < size.y)
            {
                return tiles[x * size.x + y];
            }
        }
        return null;
    }


    public void ToggleDestination(GameTile tile)
    {
        if(tile.Content.Type == GameTileContentType.Destination)
        {
            tile.Content = contentFactory.Get(GameTileContentType.Empty);
            /*
            if (!FindPaths())
            {
                tile.Content = contentFactory.Get(GameTileContentType.Destination);
                FindPaths();
            }
            */
            FindPaths();
        } else
        {
            tile.Content = contentFactory.Get(GameTileContentType.Destination);
            FindPaths();
        }
    }
}
