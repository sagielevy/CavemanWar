using UnityEngine;
using System.Collections;
using Logic;
using System;

namespace UI
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GroundFire GroundPrefab;
        [SerializeField] private LevelSettings LevelSettings;

        private LevelGenerator LevelTilesGenerator;
        private Logic.LevelLogicManager LevelLogicManager;
        private Logic.LevelGenerator LevelStateGenerator;

        private LevelState LevelState;

        private void Start()
        {
            LevelStateGenerator = new Logic.LevelGenerator();
            LevelTilesGenerator = FindObjectOfType<LevelGenerator>();
            LevelLogicManager = new LevelLogicManager(LevelSettings);

            LevelState = LevelStateGenerator.InitialState(LevelSettings);
            LevelTilesGenerator.GenerateInitialLevel(transform, LevelState, LevelSettings);
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
            throw new NotImplementedException();
        }
    }
}