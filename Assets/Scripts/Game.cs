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

    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);


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
        gameBoard.Initialize(boardSize, tileContentFactory);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }
    }

    void HandleTouch()
    {
        GameTile tile = gameBoard.GetTile(TouchRay);
        if(tile != null)
        {
            gameBoard.ToggleDestination(tile);
        }
    }
}
