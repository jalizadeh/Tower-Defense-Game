using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    //The minimum values are 2x2, so `OnValidate` makes sure of it
    [SerializeField]
    Vector2Int boardSize = new Vector2Int(11, 11);

    [SerializeField]
    GameBoard gameBoard = default;

    [SerializeField]
    GameTileContentFactory tileContentFactory = default;



    private void OnValidate()
    {
        if(boardSize.x < 2)
        {
            boardSize.x = 2;
        }

        if (boardSize.y < 2)
        {
            boardSize.y = 2;
        }
    }

    private void Awake()
    {
        gameBoard.Initialize(boardSize);
    }
}
