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

    public void Initialize(Vector2Int size)
    {
        this.size = size;
        ground.localScale = new Vector3(size.x, size.y, 1f);
        tiles = new GameTile[size.x * size.y];

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                GameTile tile = tiles[i] = Instantiate(tilePrefab,
                    new Vector3(i - (size.x - 1) * 0.5f, 0f, j - (size.y - 1) * 0.5f),
                    Quaternion.identity);
                tile.name = "Tile [" + i + "," + j + "]";
                tile.transform.SetParent(transform, false);
            }
        }
    }
}
