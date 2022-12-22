using System;
using UnityEngine;

namespace Logic 
{
    public struct LevelLogicManager
    {
        private LevelSettings levelSettings;

        public LevelLogicManager(LevelSettings levelSettings)
        {
            this.levelSettings = levelSettings;
        }

        public LevelState UpdateLevel(PlayerInput player1Input,
            PlayerInput player2Input, LevelState currLevelState, 
            float deltaTime)
        {
            /* iterate over Grid, for each cell perform these actions:
            1. add deltaTime to each
            */

            throw new NotImplementedException();
        }

        public bool IsPlayerMoving(Player playerState)
        {
            throw new NotImplementedException();
        }

        public bool IsTileBurning(Vector2Int tileIndex) 
        {
            throw new NotImplementedException();
        }

        public bool IsGameOver(LevelState levelState)
        {
            throw new NotImplementedException();
        }
    }
}
