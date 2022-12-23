using UnityEngine;
using System.Collections;
using Logic;
using System;

namespace UI
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelSettings LevelSettings;
        [SerializeField] private UICanvas UICanvas;

        private LevelGenerator LevelObjectsGenerator;
        private Logic.LevelLogicManager LevelLogicManager;
        private Logic.LevelGenerator LevelStateGenerator;

        private PlayerController player1;
        private PlayerController player2;
        private Transform[,] grid;

        private LevelState LevelState;

        private void Start()
        {
            LevelStateGenerator = new Logic.LevelGenerator();
            LevelObjectsGenerator = FindObjectOfType<LevelGenerator>();
            LevelLogicManager = new LevelLogicManager(LevelSettings);

            LevelState = LevelStateGenerator.InitialState(LevelSettings);
            var objects = LevelObjectsGenerator.GenerateInitialLevel(transform,
                LevelState, LevelSettings);
            player1 = objects.Item1;
            player2 = objects.Item2;
            grid = objects.Item3;
        }

        private void Update()
        {
            if(!UICanvas.isGameStarted || LevelLogicManager.IsGameOver(LevelState))
            {
                return;
            }

            var player1Input = GetPlayer1Input();
            var player2Input = GetPlayer2Input();
            var previousLevelState = LevelState;

            LevelState = LevelLogicManager.UpdateLevel(LevelStateGenerator,
                player1Input, player2Input, previousLevelState, Time.deltaTime);

            UpdateLevelObjects(previousLevelState);

            if (LevelLogicManager.IsGameOver(LevelState) &&
                !LevelLogicManager.IsGameOver(previousLevelState))
            {
                var playerDeadIndex = LevelLogicManager.IsPlayerDead(LevelState.player1) ? 1 : 0;
                UICanvas.endGame(playerDeadIndex);
            }
        }

        private PlayerInput GetPlayer1Input()
        {
            // WASD & Space
            var tryAttacking = false;

            if (Input.GetKey(KeyCode.Space))
            {
                tryAttacking = true;
            }

            Direction? moveDirection = null;

            if (Input.GetKey(KeyCode.W))
            {
                moveDirection = Direction.Up;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                moveDirection = Direction.Left;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveDirection = Direction.Down;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveDirection = Direction.Right;
            }

            return new PlayerInput(moveDirection, tryAttacking);
        }

        private PlayerInput GetPlayer2Input()
        {
            // Arrows and Enter
            var tryAttacking = false;

            if (Input.GetKey(KeyCode.Return))
            {
                tryAttacking = true;
            }

            Direction? moveDirection = null;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                moveDirection = Direction.Up;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveDirection = Direction.Left;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                moveDirection = Direction.Down;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                moveDirection = Direction.Right;
            }

            return new PlayerInput(moveDirection, tryAttacking);
        }

        private void UpdateLevelObjects(LevelState previousLevelState)
        {
            player1.UpdatePlayer(previousLevelState.player1, LevelState.player1,
                LevelSettings, LevelState.grid, LevelLogicManager);
            player2.UpdatePlayer(previousLevelState.player2, LevelState.player2,
                LevelSettings, LevelState.grid, LevelLogicManager);

            for (int i = 0; i < LevelState.grid.tiles.GetLength(0); i++)
            {
                for (int j = 0; j < LevelState.grid.tiles.GetLength(1); j++)
                {
                    var prevTile = previousLevelState.grid.tiles[i, j];
                    var currTile = LevelState.grid.tiles[i, j];

                    if (prevTile is Logic.BurnableTile prevBurnableTile &&
                        currTile is Logic.BurnableTile currBurnableTile)
                    {
                        BurnableTile burnableGameTile = grid[i, j].GetComponent<BurnableTile>();

                        if (LevelLogicManager.IsTileBurning(prevBurnableTile) !=
                            LevelLogicManager.IsTileBurning(currBurnableTile))
                        {
                            burnableGameTile.Burn(LevelLogicManager.IsTileBurning(currBurnableTile));
                        }

                        if (prevTile is Logic.Weed && currTile is Logic.Ground)
                        {
                            Destroy(grid[i, j].gameObject);
                            grid[i, j] = LevelObjectsGenerator.MakeGroundFire(new Vector2Int(i, j),
                                transform, LevelSettings);
                        }
                        else if (prevTile is Logic.Ground && currTile is Logic.Weed)
                        {
                            Destroy(grid[i, j].gameObject);
                            grid[i, j] = LevelObjectsGenerator.MakeWeed(new Vector2Int(i, j),
                                transform, LevelSettings);
                        }
                    }
                }
            }
        }
    }
}