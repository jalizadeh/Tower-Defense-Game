﻿using System.Collections;
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
        gameBoard.ShowGrid = true;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        } else if (Input.GetMouseButtonDown(1))
        {
            HandleAlternativeTouch();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            gameBoard.ShowPaths = !gameBoard.ShowPaths;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            gameBoard.ShowGrid = !gameBoard.ShowGrid;
        }
    }

    void HandleTouch()
    {
        GameTile tile = gameBoard.GetTile(TouchRay);
        if(tile != null)
        {
            gameBoard.ToggleWall(tile);
        }
    }

    void HandleAlternativeTouch()
    {
        GameTile tile = gameBoard.GetTile(TouchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                gameBoard.ToggleSpawnPoint(tile);
            }
            else
            {
                gameBoard.ToggleDestination(tile);
            }
        }
    }
}
