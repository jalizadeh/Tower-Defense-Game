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

    [SerializeField]
    EnemyFactory enemyFactory = default;

    [SerializeField, Range(0.1f, 10f)]
    float spawnSpeed = 1f;

    float spawnProgress;

    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    EnemyCollection enemyCollection = new EnemyCollection();


    //If anything changes in the inspector, run it
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


        enemyCollection.GameUpdate();

        spawnProgress += spawnSpeed * Time.deltaTime;
        if(spawnProgress >= 1f)
        {
            spawnProgress -= 1f;
            SpawnEnemy();
        }
    }


    void SpawnEnemy()
    {
        GameTile spawnPoint = gameBoard.GetSpawnPoint(Random.Range(0, gameBoard.spawnPointsCount));
        Enemy enemy = enemyFactory.Get();
        enemyCollection.AddEnemy(enemy);
        enemy.SpawnOn(spawnPoint);
    }


    void HandleTouch()
    {
        GameTile tile = gameBoard.GetTile(TouchRay);
        if(tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                gameBoard.ToggleTower(tile);
            }
            else
            {
                gameBoard.ToggleWall(tile);
            }
            
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
