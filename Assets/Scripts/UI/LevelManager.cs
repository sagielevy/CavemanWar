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
            throw new NotImplementedException();
        }

        private PlayerInput GetPlayer2Input()
        {
            throw new NotImplementedException();
        }

        private void UpdateLevelObjects(LevelState previousLevelState)
        {
            throw new NotImplementedException();
        }
    }
}