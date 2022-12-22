﻿using UnityEngine;
using System.Collections;
using Logic;
using System;

namespace UI
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GroundFire GroundFirePrefab;
        [SerializeField] private LevelSettings LevelSettings;

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
            var player1Input = GetPlayer1Input();
            var player2Input = GetPlayer2Input();
            var previousLevelState = LevelState;

            LevelState = LevelLogicManager.UpdateLevel(player1Input, player2Input,
                previousLevelState, Time.deltaTime);

            UpdateLevelObjects(previousLevelState);
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

            if (Input.GetKey(KeyCode.Space))
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
            player1.UpdatePlayer(LevelState.player1, previousLevelState.player1,
                LevelSettings);
            player2.UpdatePlayer(LevelState.player2, previousLevelState.player2,
                LevelSettings);

            for (int i = 0; i < LevelState.grid.tiles.GetLength(0); i++)
            {
                for (int j = 0; j < LevelState.grid.tiles.GetLength(1); j++)
                {
                    var prevTile = previousLevelState.grid.tiles[i, j];
                    var currTile = LevelState.grid.tiles[i, j];

                    throw new NotImplementedException();
                }
            }
        }
    }
}